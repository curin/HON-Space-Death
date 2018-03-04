using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FPCamController))]
public class BootController : MonoBehaviour {

	FPCamController fps;
	bool pressed;

	// Use this for initialization
	void Start () 
	{
		fps = GetComponent<FPCamController> ();
		fps.planet = FindClosestGround ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton ("EnableBoots") && !pressed)
		{
			pressed = true;
			if (fps.State == FPCamController.CharacterState.Grounded)
			{
				fps.State = FPCamController.CharacterState.Flying;
				this.transform.parent = (null);
			} 
			else
			{
				fps.State = FPCamController.CharacterState.Grounded;
				transform.parent = (fps.planet.transform);
			}
		}
			
		if (Input.GetButtonUp ("EnableBoots")) 
		{
			pressed = false;
		}
	}

	Quaternion temp;
	void FixedUpdate()
	{
		Debug.DrawRay (transform.position, transform.position - fps.planet.transform.position, Color.gray);
		if (transform.up == transform.position - fps.planet.transform.position)
			Debug.Log (transform.up != transform.position - fps.planet.transform.position);
		if (fps.State == FPCamController.CharacterState.Grounded && transform.up != transform.position - fps.planet.transform.position) 
		{
			temp = transform.localRotation; 
			transform.up =  transform.position - fps.planet.transform.position;
			if (temp.eulerAngles.y != transform.localRotation.eulerAngles.y)
			{
				transform.localRotation *= Quaternion.Euler(new Vector3(0, temp.eulerAngles.y - transform.localRotation.eulerAngles.y, 0));
			}
		}
	}

	GravityAttractor FindClosestGround()
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("Planet");
		float dist = Mathf.Infinity;
		Vector3 pos = transform.position;
		Vector3 diff;
		float curDistance;
		GameObject closest = null;
		foreach (GameObject go in gos) {
			diff = go.transform.position - pos;
			curDistance = diff.sqrMagnitude;
			if (curDistance < dist) {
				closest = go;
				dist = curDistance;
			}
		}
		return closest.GetComponent<GravityAttractor>();
	}
}
