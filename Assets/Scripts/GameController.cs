using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    //true for player 1, false for player 2
    public bool PlayerTurn{ get; set; }
    public bool GameOver { get; set; }
    [SerializeField] private GameObject Tank;
    [SerializeField] public Camera2DFollow Camera;
    private GameObject Player1;
    private GameObject Player2;
    public GameObject GuiText;
    public GameObject Background;
    public GameObject Water;
    public GameObject PlayButton;
    public GameObject TitleText;
	// Use this for initialization
	void Start () {
        
        ShowMenu();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ShowMenu()
    {
        PlayButton.SetActive(true);
        TitleText.SetActive(true);
    }

    public void Play()
    {
        PlayButton.SetActive(false);
        TitleText.SetActive(false);
        ResetGame();
    }

    //Create two tanks at random x values from -15 0 and 0 to 19
    void ResetGame()
    {
        GameOver = false;
        PlayerTurn = true;
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(p);
        }
        Destroy(GameObject.FindGameObjectWithTag("Background"));
        Destroy(GameObject.FindGameObjectWithTag("Water"));
        Instantiate(Water);
        Instantiate(Background);

        Vector3 position = transform.position;
        position.x = Random.Range(-15, 0);
        Player1 = (GameObject)Instantiate(Tank, position, Quaternion.identity);
        position.x = Random.Range(0, 19);
        Player2 = (GameObject) Instantiate(Tank, position, Quaternion.identity);


        Player1.GetComponent<Tank>().PlayerTurn = true;
        Player1.GetComponent<Tank>().ControlsEnabled = true;
        Player2.GetComponent<Tank>().PlayerTurn = false;
        Camera.target = Player1.transform;
        Camera.GetComponent<Camera2DFollow>().Enabled = true;
        GameObject.Find("WonText").GetComponent<Text>().text = "";
        GuiText.GetComponent<Text>().text = "Player 1";
    }

    public void NextTurn()
    {
        Player1.GetComponent<Tank>().ControlsEnabled = false;
        Player2.GetComponent<Tank>().ControlsEnabled = false;
        StartCoroutine(SwitchTurn());
    }
    private IEnumerator SwitchTurn()
    {
        yield return new WaitForSeconds(3.5f);
        PlayerTurn = !PlayerTurn;
        if (PlayerTurn)
        {
            Player1.GetComponent<Tank>().ControlsEnabled = true;
        }
        else Player2.GetComponent<Tank>().ControlsEnabled = true;

        GuiText.GetComponent<Text>().text = PlayerTurn ? "Player 1" : "Player 2";
        Camera.target = PlayerTurn ? Player1.transform : Player2.transform;
        Camera.GetComponent<Camera2DFollow>().Enabled = true;

    }
    public void EndGame(bool playerTurn)
    {
        GameOver = true;
        GameObject.Find("WonText").GetComponent<Text>().text = playerTurn ? "Player 2 Won!" : "Player 1 Won!";
        ShowMenu();
    }

}
