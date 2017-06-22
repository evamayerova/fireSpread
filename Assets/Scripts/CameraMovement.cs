using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private Vector3 currPosition;
    private bool moving;
	// Use this for initialization
	void Start () {
        moving = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("hello");
            currPosition = Input.mousePosition;
            moving = true;
        }
        if (Input.GetMouseButtonUp(2))
        {
            moving = false;
        }

        if (moving && Input.mousePosition != currPosition)
        {
            float delta = 0.1f;
            float yaw = currPosition.x - Input.mousePosition.x;
            float pitch = currPosition.y - Input.mousePosition.y;
            Debug.Log(pitch + yaw);
            transform.rotation = Quaternion.Euler(pitch * delta, -yaw * delta, 0.0f);
            // transform.Rotate(new Vector3(pitch * delta, -yaw * delta, 0.0f));
            // transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), pitch * delta);
            // transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -yaw * delta);
        }
    }   
}
