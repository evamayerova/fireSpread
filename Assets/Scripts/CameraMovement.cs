﻿using UnityEngine;
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
        }
    }   
}
