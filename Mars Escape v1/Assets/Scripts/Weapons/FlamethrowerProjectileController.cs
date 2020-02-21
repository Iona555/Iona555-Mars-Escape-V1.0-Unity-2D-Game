using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerProjectileController : MonoBehaviour {

	Rigidbody2D flamethrowerRB;

	// Occurs when the object first comes to life
	void Awake () {
		flamethrowerRB = this.GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		flamethrowerRB.velocity = transform.parent.gameObject.GetComponent <Rigidbody2D> ().velocity;
	}
}
