using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreController : MonoBehaviour {

	// Use this for initialization
	public static int kills = 0;
	public Text text;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text ="KILLS: " + kills.ToString ();
	}
}
