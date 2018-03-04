using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour {

	public const int maxHealth = 100;
	public int currentHealth = maxHealth;
	public RectTransform healthBar;
	public Image image;
	public Text text;
	public Transform respawnLoc;
	public Transform playerLoc;

	private float alphaFade;
	private float fadeSpeed = 1f;
	private bool isDead = false;

	void Start(){
		alphaFade = 0;
		image.GetComponent<CanvasRenderer>().SetAlpha(0f);
		text.GetComponent<CanvasRenderer>().SetAlpha(0f);
	}

	void Update(){
		healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
		//GameObject.Find ("Dead").GetComponent<CanvasRenderer> ().SetAlpha (0);
		image.GetComponent<CanvasRenderer>().SetAlpha(alphaFade);
		text.GetComponent<CanvasRenderer>().SetAlpha(alphaFade);
		if (isDead) {
			Dead ();
		} else {
			alphaFade = 0;
		}
	}

	public void TakeDamage(int amount){
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			Debug.Log ("Dead");
			isDead = true;
		}

		//healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
	}

	void Dead(){
		//image.GetComponent<CanvasRenderer>().SetAlpha(alphaFade);
		//text.GetComponent<CanvasRenderer>().SetAlpha(alphaFade);
		//Watch the 2 might max at 1
		if (alphaFade < 2){
			alphaFade += .01f;
			return;
		}
			
		enemySpawner.DestroyAll ();
		gravityController.died = true;

		Spawner.Enabled = false;

		playerLoc.position = respawnLoc.position;
		currentHealth = 100;
		isDead = false;
	}
}
