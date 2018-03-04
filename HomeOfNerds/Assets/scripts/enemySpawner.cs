using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour {

	public static Dictionary<int, List<enemySpawner>> Spawners = new Dictionary<int, List<enemySpawner>> ();

	public int SpawnerGroup = 0;

	public GameObject enemyPrefab;

	public static List<enemyAI> SpawnedEnemies = new List<enemyAI>();

	// Use this for initialization
	void Awake () 
	{
		if (!Spawners.ContainsKey (SpawnerGroup))
			Spawners.Add (SpawnerGroup, new List<enemySpawner> ());

		Spawners [SpawnerGroup].Add (this);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void SpawnEnemy()
	{
		enemyAI ai = Instantiate (enemyPrefab, transform.position, transform.rotation).GetComponent<enemyAI>();
		SpawnedEnemies.Add(ai);
	}

	public static void DestroyAll()
	{
		foreach (enemyAI obj in SpawnedEnemies)
		{
			for (int i = 0; i < obj.transform.childCount; i++)
				Destroy (obj.transform.GetChild (i).gameObject);
			Destroy (obj.gameObject);
		}
		SpawnedEnemies.Clear ();
	}
}
