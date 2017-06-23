using UnityEngine;
using System.Collections;
using System;

public class WindDirectionFollower : MonoBehaviour {

    private float currentAngle;

    void Start()
    {
        currentAngle = 0.0f;
    }

	public void changeWindDirection(float degrees)
    {
        gameObject.transform.Rotate(new Vector3(0, 0, (degrees - currentAngle)));
        currentAngle = degrees;
    }
    
}
