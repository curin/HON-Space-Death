using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpenOnCollision : MonoBehaviour {

	//public Animator animator;
	public GameObject doorDoor;
	public Transform positionOpen;
	public Transform startPos;

	private bool IsOpen;
	private bool IsClosed;
	private float speed = 1f;
	public int openCounter = 0;

	void Start () {
		IsClosed = false;
		IsOpen = false;
		doorDoor.transform.position = startPos.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//animator.SetBool ("open", IsOpen);
		//animator.SetBool ("closed", IsClosed);


		if (IsOpen == true) {
			float step = speed * Time.deltaTime * .01f;
			//doorDoor.transform.position = Vector3.MoveTowards (transform.position, positionOpen.position, .2f);
			GetComponent<MeshRenderer> ().material.color = Color.green;
			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.green);
			doorDoor.transform.position = Vector3.Lerp (transform.position, positionOpen.position, 1f);

			if(openCounter < 600){
				openCounter += 1;
				return;
			}
			IsOpen = false;

		} else {
			GetComponent<MeshRenderer> ().material.color = Color.red;
			GetComponent<MeshRenderer> ().material.SetColor ("_EmissionColor", Color.red);
			doorDoor.transform.position = Vector3.Lerp (transform.position, startPos.position, 100f);
			openCounter = 0;
		}

	}

	/**void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "bullet") {
			IsOpen = true;
			Debug.Log ("switch hit");
		}
	}**/

	void OnTriggerEnter(Collider other){
		Debug.Log ("ButtonHit");
		IsOpen = true;
		//if (other.tag == "bullet") {
		//	Debug.Log ("ButtonHit");
	//	}
	}
}
