using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingScript : MonoBehaviour {

	Rigidbody body;
	CharacterController charCont;

	public float Speed;
	public float Speed2;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		charCont = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Speed = body.velocity.magnitude;
		Speed2 = charCont.velocity.magnitude;
	}
}
