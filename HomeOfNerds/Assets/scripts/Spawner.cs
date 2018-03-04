using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public static bool Enabled = false;
	public static int Waves = 1;

	// Use this for initialization
	void Start () {
		SpawnTimer = Random.Range(600, 1200);
	}

	int waves = Waves;
	float tick = 0;
	float SpawnTimer;
	// Update is called once per frame
	void Update () {
		if (!Enabled)
			tick = 0;
		else
		{
			if (waves == 0)
				waves = Waves;
			tick++;
			if (tick >= SpawnTimer)
			{
				SpawnTimer = Random.Range(600, 1200);
				tick = 0;
				waves--;

				Debug.Log (tick + "/" + SpawnTimer);

				if (waves == 0)
					Enabled = false;

				int i = Random.Range (0, enemySpawner.Spawners.Keys.Count - 1);

				foreach (enemySpawner spawn in enemySpawner.Spawners[i])
				{
					spawn.SpawnEnemy ();
				}
			}
		}
	}
}
