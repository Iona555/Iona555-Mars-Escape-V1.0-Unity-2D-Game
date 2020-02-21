using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFireballScript : MonoBehaviour {

	public float fireballDamage;
	FireballProjectileController FPC;
	public GameObject explosionEffect;

	// Use this for initialization
	void Awake () {
		FPC = GetComponentInParent <FireballProjectileController> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Cosmonaut") {
			other.GetComponent<cosmonautBase> ().takeDamage (fireballDamage);
			Instantiate (explosionEffect, transform.position, transform.rotation);
			FPC.fireballHit = true;
			Destroy (gameObject);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			Instantiate (explosionEffect, transform.position, transform.rotation);
			FPC.fireballHit = true;
			Destroy (gameObject);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Cosmonaut") {
			other.GetComponent<cosmonautBase> ().takeDamage (fireballDamage);
			Instantiate (explosionEffect, transform.position, transform.rotation);
			FPC.fireballHit = true;
			Destroy (gameObject);
		}
		if (other.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			Instantiate (explosionEffect, transform.position, transform.rotation);
			FPC.fireballHit = true;
			Destroy (gameObject);
		}
	}
}
