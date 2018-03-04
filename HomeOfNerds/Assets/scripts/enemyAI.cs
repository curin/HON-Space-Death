using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyAI : MonoBehaviour {

	public enum AIType
	{
		Retriever,
		Fighter
	}

	/** TODO AI SETS RANDOM ASTROID WITHIN RADIUS TO "DASH" TO. ADD PARTICLE EFFECT TOO**/

	public AIType type;
	public Transform target;
	public float speed;
	public Vector3 targetDir;
	public bool clearPath = true;
	private BaseAI actingAI;
	public GameObject BulletPrefab;
	//public GameObject killCounter;
	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag ("Player").transform;
		switch (type)
		{
		case AIType.Retriever:
			actingAI = new RetrieverAI ();
			break;
		case AIType.Fighter:
			actingAI = new FighterAI ();
			break;
		default:
			actingAI = new RetrieverAI ();
			break;
		}
		actingAI.Parent = this;
		actingAI.Start ();

		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
	}

	bool BeenInRange = false;
	// Update is called once per frame
	void Update () 
	{
		targetDir = target.position - transform.position;
		if (targetDir.magnitude <= actingAI.Range)
		{
			actingAI.InRange = true;
			if (!BeenInRange)
			{
				BeenInRange = true;
				actingAI.OnRangeEnter ();
			}
			actingAI.WhileInRange ();
		} 
		else
		{
			actingAI.InRange = false;
			if (BeenInRange)
			{
				BeenInRange = false;
				actingAI.OnRangeExit ();
			}
		}

		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, actingAI.Speed * Time.deltaTime, 0.0F) * 10;
		transform.rotation = Quaternion.LookRotation (newDir);
		Debug.DrawRay (transform.position, newDir, Color.green);

		actingAI.Pathing ();
	}

	public void TakeDamage(float damage)
	{
		actingAI.Health -= damage;
		if (actingAI.Health < 0)
		{
			actingAI.OnDeath ();
			//killCounter.GetComponent<scoreController> ().kills += 1;
			for (int i = 0; i < transform.childCount; i++)
				Destroy (transform.GetChild (i).gameObject);

			enemySpawner.SpawnedEnemies.Remove (this);

			if (enemySpawner.SpawnedEnemies.Count == 0)
			{
				Spawner.Waves++;
				Spawner.Enabled = true;
			}

			Destroy (this.gameObject);

			scoreController.kills++;
		}
	}

	public void PathToTarget(float speed){
		float step = speed * Time.deltaTime;
		targetDir = target.position - transform.position;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, 3))
		{
			transform.position += Vector3.up * Time.deltaTime * 5;
		} 
		else
		{
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
		}
	}
}
