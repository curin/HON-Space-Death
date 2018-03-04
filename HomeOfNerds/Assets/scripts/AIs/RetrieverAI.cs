using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieverAI : BaseAI
{
	bool grabbed = false;
	GameObject child;
	public float MaxHealth 
	{ 
		get; 
		set;
	}

	public float Health 
	{ 
		get; 
		set;
	}

	public float Range 
	{ 
		get;
		set;
	}

	public float Speed 
	{ 
		get; 
		set; 
	}

	public bool InRange 
	{ 
		get; 
		set; 
	}

	public enemyAI Parent 
	{ 
		get;
		set;
	}

	public void Start() 
	{
		MaxHealth = 5;
		Health = MaxHealth;
		Range = 1;
		Speed = 10;
	}

	public void Pathing()
	{
		Parent.PathToTarget(Speed);
	}

	public void OnRangeEnter()
	{
		if (!grabbed)
		{
			Parent.target.parent = Parent.transform;
			child = Parent.target.gameObject;
			Parent.target = GameObject.FindGameObjectWithTag ("EnemyBase").transform;
		} 
		else
		{
			GameObject.Destroy (child);
			GameObject.Destroy (Parent.gameObject);
		}
	}

	public void OnRangeExit ()
	{
		
	}

	public void WhileInRange()
	{
		
	}

	public void OnDeath ()
	{
		
	}
}
