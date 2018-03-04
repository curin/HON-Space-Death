using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class gravityController : MonoBehaviour {

	public Rigidbody body;

	public static bool died = true;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other){
		//Debug.Log ("enter");
		if (other.tag == "Player") {
		//	Debug.Log ("enter");
			body = other.GetComponent<Rigidbody> ();
			other.GetComponent<CharacterController> ().enabled = true;
			other.GetComponent<FPCamController> ().enabled = false;
			other.GetComponent<FirstPersonController> ().enabled = true;
			//body.isKinematic = true;
			//body.useGravity = true;
		}
	}

	void OnTriggerExit(Collider other){
	//	Debug.Log ("left");
		if (other.tag == "Player" && !other.isTrigger) 
		{
			Debug.Log ("left");
			body = other.GetComponent<Rigidbody> ();
			body.velocity = other.GetComponent<CharacterController> ().velocity;
			other.GetComponent<CharacterController> ().enabled = false;
			other.GetComponent<FPCamController> ().enabled = true;
			other.GetComponent<FirstPersonController> ().enabled = false;
			//body.isKinematic = false;
			//body.useGravity = false;

			if (died)
			{
				foreach (enemySpawner spawn in enemySpawner.Spawners[0])
					spawn.SpawnEnemy ();
				died = false;
			}
		}
	}
}
