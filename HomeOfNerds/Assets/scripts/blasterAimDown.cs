using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blasterAimDown : MonoBehaviour {

	public Camera FPCam;
	public float ZoomSpeed = 2;
	public bool IsZoom = false;
	public Animator animator;

	private float zoomFov = 35;
	private float regFov = 50;
	private float currentFov = 50;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		playerHealthDebug ();
		mouseZoom ();
		animator.SetBool ("zoom", IsZoom);
	}

	void mouseZoom()
	{
		FPCam.fieldOfView = currentFov;

		if (Input.GetAxis("Fire2") > 0) {
			IsZoom = true;
			if (currentFov > zoomFov) 
			{
				currentFov = currentFov - ZoomSpeed;
			}
		} 
		else 
		{
			IsZoom = false;
			if (currentFov < regFov)
			{
				currentFov = currentFov + ZoomSpeed;
			}
		}
	}

	void playerHealthDebug(){
		if (Input.GetKeyDown (KeyCode.P)) {
			var health = GameObject.Find("Player").GetComponent<playerHealth> ();

			if (health != null) {
				health.TakeDamage (10);
				Debug.Log ("take damage");
			}
		}
	}
}
