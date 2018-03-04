using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		Vector3 pos = this.transform.position;
		var hit = collision.gameObject;
		var health = hit.GetComponent<playerHealth> ();
		if (health != null) {
			health.TakeDamage (10);
			Debug.Log ("take damage");
		}

		var Enemy = hit.GetComponent<enemyAI> ();
		if (Enemy != null)
		{
			Enemy.TakeDamage (5);
		}
		//Debug.Log (pos.ToString());
		Destroy (gameObject); 

		//foreach (ContactPoint contact in collision.contacts) {
			//Debug.DrawRay (contact.point, contact.normal, Color.white);
		//}
	}
}
