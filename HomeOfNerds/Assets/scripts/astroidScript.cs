using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astroidScript : MonoBehaviour {

	private float angularVelocity = 10.0f;
	private Vector3 axisOfRotation;
	// Use this for initialization
	void Start () {
		axisOfRotation = Random.onUnitSphere;
		angularVelocity *= Random.value;
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.rotation = Random.rotation;
		transform.Rotate(axisOfRotation, angularVelocity * Time.smoothDeltaTime);
	}
}
