using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectileController : MonoBehaviour {

	Rigidbody2D fireballRB;
	public float fireballSpeed;
	Vector2 fireballVector;
	float fireballVectorX;

	GameStateMachine GSM;

	public bool fireballHit = false;

	// Occurs when the object first comes to life
	void Awake () {
		fireballRB = this.GetComponent<Rigidbody2D> ();
		fireballSpeed = 4f;

		// Adjust the vector
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
		if (GSM.Maraptors[GSM.currentMaraptor].GetComponent<maraptorController> ().facingRight)
			fireballVectorX = 1;
		else
			fireballVectorX = -1;
		fireballVector = new Vector2 (fireballVectorX, 0);
	}

	// Update is called once per frame
	void Update () {
		if (!fireballHit) {
			fireballSpeed += 1;
			fireballRB.AddForce (fireballVector * fireballSpeed, ForceMode2D.Force);
		} else {
			fireballRB.velocity = new Vector2 (0, 0);
			StartCoroutine("DestroyTimerIncrementor");
		}
	}

	private IEnumerator DestroyTimerIncrementor()
	{
		while (true) {
			yield return new WaitForSeconds (0.7f);
			Destroy (gameObject);
		}
	}
}
