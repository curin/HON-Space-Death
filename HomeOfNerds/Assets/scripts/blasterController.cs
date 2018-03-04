using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blasterController : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	public bool fired = false;
	public int bulletSpeed;

	public int tick = 0;
	// Use this for initialization
	void Start () {
		bulletSpeed = 200;
	}
	
	// Update is called once per frame
	void Update () 
	{
		tick++;
		if (Input.GetAxis ("Fire1") > 0 && !fired) 
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Fire ();
			fired = true;
		} 
		else if (Input.GetAxis ("Fire1") == 0) 
		{
			fired = false;
			tick = 0;
		}
	}

	void Fire(){
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;

		Destroy (bullet, 10.0f);
	}
}
