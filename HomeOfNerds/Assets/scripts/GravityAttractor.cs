using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour {
	
	public float gravity = -9.8f;

	void Start()
	{
		
	}
	
	public void Attract(Rigidbody body, bool alignToNormal) {
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;
		
		// Apply downwards gravity to body
		body.AddForce((gravityUp * gravity) / Mathf.Pow((body.position - transform.position).magnitude, 1/5), ForceMode.Acceleration);

		if (alignToNormal)
		{
			// Allign bodies up axis with the centre of planet
			body.rotation = Quaternion.FromToRotation (localUp, gravityUp) * body.rotation;
		}
	}  
}
