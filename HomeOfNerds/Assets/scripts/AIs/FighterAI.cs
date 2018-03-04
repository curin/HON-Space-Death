using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : BaseAI {
	public float bulletSpeed;
	public float firingSpeed;
	public float firingRange;

	public float MaxHealth { get; set; }
	public float Health { get; set; }
	public float Range { get; set; }
	public float Speed { get; set; }
	public bool InRange { get; set; }
	public enemyAI Parent { get; set; }

	bool dir = false;
	bool dir2 = false;
	bool newObstacle = true;

	public void Start()
	{
		MaxHealth = 10 + Random.Range (-5, 5);
		Health = MaxHealth;
		Range = 120 + Random.Range (-30, 30);
		Speed = (10 + Random.Range (-3, 5)) * 2.5f;
		firingRange = 240;
		firingSpeed = 30 + Random.Range (-5, 5);
		bulletSpeed = 200;
		dir = Random.value > .5f;
	}

	float tick = 0;
	public void Pathing()
	{
		if (InRange)
		{
			if (tick < firingSpeed)
				tick++;
			else
			{
				tick -= firingSpeed;
				Fire ();
			}

			RaycastHit hit;


			if (Physics.Raycast (Parent.transform.position, Parent.transform.right, out hit, 3))
			{
				if (newObstacle)
				{
					dir2 = Random.value > .5f;
					newObstacle = false;
				}

				Parent.transform.position += (dir2 ? 1 : -1) * Vector3.up * Time.deltaTime * 5;
			} 
			else
			{
				newObstacle = true;
				Parent.transform.position += (dir ? 1 : -1) * Parent.transform.right * Time.deltaTime * Speed;
			}
		}
		else
		{
			
			Parent.PathToTarget (Speed);

			if (Parent.targetDir.magnitude < firingRange)
			{
				if (tick < firingSpeed)
					tick++;
				else
				{
					tick -= firingSpeed;
					Fire ();
				}
			}
		}
	}

	public void OnRangeEnter()
	{
		Speed /= 2;
	}

	public void OnRangeExit ()
	{
		Speed *= 2;
	}
		
	public void WhileInRange()
	{
		
	}

	public void OnDeath ()
	{

	}

	void Fire(){
		//Debug.Log ("BAM");
		Quaternion lead = Quaternion.FromToRotation (Parent.target.transform.position - Parent.transform.position, Parent.target.transform.position  - Parent.transform.position);
		//lead = Quaternion.Euler (lead.eulerAngles * Parent.target.GetComponent<Rigidbody> ().velocity.magnitude );
		var bullet = (GameObject)GameObject.Instantiate (
			Parent.BulletPrefab,
			Parent.transform.position + Parent.transform.forward,
			Parent.transform.rotation * lead);

		bullet.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * bulletSpeed;

		GameObject.Destroy (bullet, firingRange/24);
	}
}
