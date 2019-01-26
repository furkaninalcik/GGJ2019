using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFoat : MonoBehaviour {

    float velocity = 1, frequency = 1, amplitude = 1;
    Vector3 tempPosition;

	void Start () {
        tempPosition = this.transform.position;
        Debug.Log(tempPosition);
	}
	
	void Update () {
        tempPosition.x += velocity;
        tempPosition.y += Mathf.Sin(Time.realtimeSinceStartup * frequency) * amplitude;
        this.transform.position = tempPosition;
	}
}
