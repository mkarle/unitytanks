using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    private float top = 3.73f, right = 16.15f, bottom = -3.4f, left = -20.1f; 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float x = 0, y = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > left)
            x = -1;
        else if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < right)
            x = 1;
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < top)
            y = 1;
        else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > bottom)
            y = -1;
        if (Camera.current != null && (x != 0 || y != 0))
        {
            gameObject.GetComponent<Camera2DFollow>().Enabled = false;
            Camera.current.transform.Translate(new Vector2(x * 0.5f, y * 0.5f));
        }
    }


}
