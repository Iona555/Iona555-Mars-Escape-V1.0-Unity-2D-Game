using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamebreathProjectileController : MonoBehaviour {

	Rigidbody2D flamebreathRB;

	// Occurs when the object first comes to life
	void Awake () {
		flamebreathRB = this.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		flamebreathRB.velocity = transform.parent.gameObject.GetComponent <Rigidbody2D> ().velocity;
	}
}
