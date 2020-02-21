using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]	// To make the script visible
public class maraptorBase : MonoBehaviour {
	// Name
	public string characterName;

	// Health and Shield Status
	public float baseHP;
	public float currentHP;
	public float baseShield;
	public float currentShield;
	public bool isDead = false;

	// Buffs
	public bool jumpBoost;

	// Equiped weapons
	public bool flameBreath = false;
	public bool fireball = false;

	// Items
	public int healUsages;
	public int shieldUsages;
	public int jumpBoostUsages;
	public int flameBreathUsages;
	public int fireballUsages;

	public Vector3 currentPosition;

	public GameObject FloatingTextPrefab;

	public GameStateMachine GSM;

	void Start () {
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
	}

	public void takeDamage(float damage)
	{
		damage = (int)damage;
		if (FloatingTextPrefab)
		{
			var go = Instantiate (FloatingTextPrefab, transform.position, Quaternion.identity, transform);
			float gcg = 2.55f * damage;
			if (gcg > 255.0f)
				gcg = 255.0f;
			go.GetComponent<TextMesh> ().color = new Color (255.0f/255.0f, gcg/255.0f, 0.0f/255.0f);
			go.GetComponent<TextMesh> ().text = ("-" + damage.ToString ());
		}
		currentShield -= damage;
		if (currentShield < 0) {
			currentHP += currentShield;
			currentShield = 0;
			if (currentHP < 20 && GSM.firstTurn) {
				currentHP = 20;
			} else {
				if (currentHP < 0) {
					isDead = true;
					currentHP = 0;
				}
			}
		}
	}

	public void takeHeal(float heal)
	{
		heal = (int)heal;
		if (FloatingTextPrefab) {
			var go = Instantiate (FloatingTextPrefab, transform.position, Quaternion.identity, transform);
			go.GetComponent<TextMesh> ().color = new Color (0.0f/255.0f, 250.0f/255.0f, 0.0f/255.0f);
			go.GetComponent<TextMesh> ().text = ("+" + heal.ToString ());
		}
		if (currentHP < baseHP) {
			currentHP += heal;
			if (currentHP > baseHP) {
				heal = currentHP - baseHP;
				currentHP = baseHP;
			} else {
				heal = 0;
			}
		}
		if (currentShield < baseShield) {
			currentShield += heal;
			if (currentShield > baseShield) {
				// The rest of the heal is lost
				currentShield = baseShield;
			}
		}
	}
}
