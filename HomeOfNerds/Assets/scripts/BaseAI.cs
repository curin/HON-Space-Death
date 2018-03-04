using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseAI {
	float MaxHealth { get; set; }
	float Health { get; set; }
	float Range { get; set; }
	float Speed { get; set; }
	bool InRange { get; set; }
	enemyAI Parent { get; set; }
	void Start();
	void Pathing();
	void OnRangeEnter();
	void OnRangeExit ();
	void WhileInRange();
	void OnDeath ();
}
