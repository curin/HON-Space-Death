using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicRotation : MonoBehaviour {

	// Use this for initialization

	public float speed;
	public float x;
	public float y;
	public float z;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(x * Time.deltaTime * speed, y * Time.deltaTime * speed, z * Time.deltaTime * speed);
	}
}
