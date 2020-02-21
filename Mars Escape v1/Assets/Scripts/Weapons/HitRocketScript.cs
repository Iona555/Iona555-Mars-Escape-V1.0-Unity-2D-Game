using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRocketScript : MonoBehaviour {

	public float rocketDamage;
	RocketProjectileController RPC;
	public GameObject explosionEffect;

	// Use this for initialization
	void Awake () {
		RPC = GetComponentInParent <RocketProjectileController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Maraptor") {
			other.GetComponent<maraptorBase> ().takeDamage (rocketDamage);
			Instantiate (explosionEffect, transform.position, transform.rotation);
			RPC.rocketHit = true;
			Destroy (gameObject);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			Instantiate (explosionEffect, transform.position, transform.rotation);
			RPC.rocketHit = true;
			Destroy (gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Maraptor") {
			RPC.rocketHit = true;
			Instantiate (explosionEffect, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			RPC.rocketHit = true;
			Instantiate (explosionEffect, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
