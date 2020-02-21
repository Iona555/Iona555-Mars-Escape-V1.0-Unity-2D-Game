using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour {
	// Crate variables
	public bool crateReady;
	public bool crateJustSpawned;
	public bool crateJustPicked;
	public GameObject crate;

	// To be spawned
	public GameObject Crate_Health;
	public GameObject Crate_JumpBoost;
	public GameObject Crate_Shield;
	public GameObject Crate_Flame;
	public GameObject Crate_Explosion;

	// The collection of crate objects
	GameObject CratesCollection;

	// The crate ready effect
	public ParticleSystem CrateReadyEffect;

	// Cooldown bar
	GameObject CooldownBar;
	public Vector3 newLocalScale;
	float initialScaleX;

	// Timer variables
	public float CooldownDuration;
	public float CooldownTimer;
	public bool CharacterTooClose;

	// Use this for initialization
	void Start () {
		CrateReadyEffect = this.transform.GetChild(0).GetComponent<ParticleSystem> ();
		CooldownBar = this.transform.GetChild (1).gameObject;

		CratesCollection = GameObject.Find ("CratesCollection");
		crateReady = false;
		crateJustSpawned = false;
		crateJustPicked = true;
		CrateReadyEffect.Stop ();
		CooldownDuration = 5;
		CooldownTimer = 0;
		CharacterTooClose = false;

		newLocalScale = CooldownBar.transform.localScale;
		newLocalScale.x = 0;
		initialScaleX = CooldownBar.transform.localScale.x;
		CooldownBar.transform.localScale = newLocalScale;
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the spawner has a crate ready
		if (crateReady == false && crate != null) {
			crateReady = true;
			crateJustSpawned = true;
		} else if (crateReady == true && crate == null) {
			crateReady = false;
			crateJustPicked = true;
		}
			
		if (crateJustSpawned) {
			crateJustSpawned = false;
			// Start the crate ready effect
			CrateReadyEffect.Play ();
			// Set the color of the effect
			ParticleSystem.MainModule psSettings = CrateReadyEffect.main;
			switch (crate.tag) {
			case "Crate_Health":
				psSettings.startColor = new ParticleSystem.MinMaxGradient (
					new Color (255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f) /*Red*/,
					new Color (255.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f) /*Pink*/);
				break;
			case "Crate_JumpBoost":
				psSettings.startColor = new ParticleSystem.MinMaxGradient (
					new Color (0.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f) /*Green*/,
					new Color (0.0f / 255.0f, 255.0f / 255.0f, 200.0f / 255.0f) /*Green - Blue*/);
				break;
			case "Crate_Shield":
				psSettings.startColor = new ParticleSystem.MinMaxGradient (
					new Color (200.0f / 255.0f, 255.0f / 255.0f, 235.0f / 255.0f) /*White - Blue - Green*/,
					new Color (0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f) /*Blue - Green*/);
				break;
			case "Crate_Flame":
				psSettings.startColor = new ParticleSystem.MinMaxGradient (
					new Color (255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f) /*Red*/,
					new Color (0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f) /*Blue*/);
				break;
			case "Crate_Explosion":
				psSettings.startColor = new ParticleSystem.MinMaxGradient (
					new Color (255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f) /*Yellow*/,
					new Color (255.0f / 255.0f, 165.0f / 255.0f, 0.0f / 255.0f) /*Orange*/);
				break;
			}
		} else if(crateJustPicked) {
			crateJustPicked = false;
			// Stop the crate ready effect
			CrateReadyEffect.Stop ();
			// Start the cooldown to spawn a crate
			StartCoroutine ("TimerDecrementor");
		}

		// Update the cooldown bar
		newLocalScale.x = (float)(initialScaleX * CooldownTimer / CooldownDuration);
		CooldownBar.transform.localScale = newLocalScale;
	}

	IEnumerator TimerDecrementor() {
		CooldownTimer = CooldownDuration;
		while (CooldownTimer > 0) {
			yield return new WaitForSeconds (0.1f);
			if(!CharacterTooClose)
				CooldownTimer -= 0.1f;
		}
		CooldownTimer = 0;
		// Spawn a crate
		SpawnCrate();
	}

	void SpawnCrate () {
		Vector3 offset = new Vector3 (0.0f, 0.2f, 0.0f);
		int RandomCrateNumber = (int)(System.DateTime.Now.Millisecond%5);
		switch (RandomCrateNumber) {
		case 0:
			(crate = Instantiate(Crate_Health, this.transform.position + offset, Quaternion.Euler (new Vector3 (0, 0, 0)))).transform.parent = CratesCollection.transform;
			break;
		case 1:
			(crate = Instantiate(Crate_JumpBoost, this.transform.position + offset, Quaternion.Euler (new Vector3 (0, 0, 0)))).transform.parent = CratesCollection.transform;
			break;
		case 2:
			(crate = Instantiate(Crate_Shield, this.transform.position + offset, Quaternion.Euler (new Vector3 (0, 0, 0)))).transform.parent = CratesCollection.transform;
			break;
		case 3:
			(crate = Instantiate(Crate_Flame, this.transform.position + offset, Quaternion.Euler (new Vector3 (0, 0, 0)))).transform.parent = CratesCollection.transform;
			break;
		case 4:
			(crate = Instantiate(Crate_Explosion, this.transform.position + offset, Quaternion.Euler (new Vector3 (0, 0, 0)))).transform.parent = CratesCollection.transform;
			break;
		}
	}
}
