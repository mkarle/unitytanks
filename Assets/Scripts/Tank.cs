using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Tank : MonoBehaviour {
    [SerializeField] private int health = 100;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float maxBulletSpeed = 500f;
    [SerializeField] private GameObject deadTank;
    public GameObject healthBar;
    private GameController gameController; 
    private Animator fireBarrelAnim = null;
    private Animator tankMoveAnim = null;

    private Transform barrel = null;
    private Transform barrelTip = null;
    private Transform barrelRotation = null;

    private int fireHash;
    private int moveHash;

    private int firePower = 0;
    public Text PowerText;
    public bool ControlsEnabled {
        get; set; }
    private PlatformerCharacter2D m_Character;
    public bool PlayerTurn {get; set; }
	// Use this for initialization
	void Start () {
        PowerText = GameObject.Find("PowerText").GetComponent<Text>();
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();

        barrel = transform.GetChild(0);
        barrelTip = barrel.GetChild(0);
        barrelRotation = transform.GetChild(1);

        fireBarrelAnim = barrel.GetComponent<Animator>();
        tankMoveAnim = GetComponent<Animator>();

        fireHash = Animator.StringToHash("FireBullet");
        moveHash = Animator.StringToHash("TankMove");

        healthBar.SetActive(false);
       
    }

    void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ControlsEnabled && gameController.PlayerTurn == PlayerTurn && !gameController.GameOver)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                IncreasePower();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                FireBullet();
            }
            if (Input.GetKey(KeyCode.S))
            {
                RotateBarrel(-1);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                RotateBarrel(1);
            }

            if (health <= 0)
            {
                Die();
            }

        }
	}

    void FixedUpdate()
    {
        if (ControlsEnabled && gameController.PlayerTurn == PlayerTurn && !gameController.GameOver)
        {
            float h = 0f;
            if (Input.GetKey(KeyCode.A))
                h = -1f;
            else if (Input.GetKey(KeyCode.D))
                h = 1f;
            // Pass all parameters to the character control script.
            if(h != 0)
            {
                tankMoveAnim.Play(moveHash);
            }
            m_Character.Move(h);

        }
    }

    void FireBullet()
    {
        fireBarrelAnim.Play(fireHash);
        int direction = transform.localScale.x >= 0 ? 1 : -1;
        GameObject newBullet = (GameObject) Instantiate(bullet, barrelTip.position, barrel.rotation);
        newBullet.GetComponent<Rigidbody2D>().AddForce(direction * barrelTip.right * maxBulletSpeed * firePower / 100f);
        firePower = 0;
        PowerText.text = "";
        gameController.Camera.target = newBullet.transform;
        gameController.NextTurn();
    }

    void IncreasePower()
    {
        if (firePower < 100)
        {
            firePower += 1;
            PowerText.text = "Power: " + firePower;
        }
    }

    void RotateBarrel(int direction)
    {
        direction = transform.localScale.x >= 0 ? direction * 1 : direction * -1;
        barrel.RotateAround(barrelRotation.position, direction * Vector3.forward, 20 * Time.deltaTime);
        if (barrel.localEulerAngles.z < 0 || barrel.localEulerAngles.z > 180)
            barrel.RotateAround(barrelRotation.position, -direction * Vector3.forward, 20 * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag.Equals("Bullet"))
        {
            Hit(20);
        }
        if (coll.gameObject.tag.Equals("Water"))
        {
            Die();
        }

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Equals("Explosion"))
        {
            Hit(10);
        }

    }

    void Hit(int damage)
    {
        health -= damage;
       // if (!healthBar.activeInHierarchy)
        {
            healthBar.SetActive(true);
        }
        Vector3 scale = healthBar.transform.localScale;
        scale.x = health / 100.0f * 0.18f;
        healthBar.transform.localScale = scale;
    }

    void Die()
    {
        Instantiate(deadTank, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
        gameController.EndGame(PlayerTurn);
    }
}

