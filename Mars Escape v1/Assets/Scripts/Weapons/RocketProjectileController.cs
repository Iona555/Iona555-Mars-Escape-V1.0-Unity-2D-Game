using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileController : MonoBehaviour {

	Rigidbody2D rocketRB;
	public float rocketSpeed;
	Vector2 rocketVector;
	float rocketVectorX;

	GameStateMachine GSM;

	public bool rocketHit = false;

	// Occurs when the object first comes to life
	void Awake () {
		rocketRB = this.GetComponent<Rigidbody2D> ();
		rocketSpeed = 1.6f;

		// Adjust the vector
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
		if (GSM.Cosmonauts[GSM.currentCosmonaut].GetComponent<cosmonautController> ().facingRight)
			rocketVectorX = 1;
		else
			rocketVectorX = -1;
		rocketVector = new Vector2 (rocketVectorX, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (!rocketHit) {
			if (rocketSpeed < 3f)
				rocketSpeed += 0.08f;
			else if (rocketSpeed < 40)
				rocketSpeed += 2f;
			rocketRB.AddForce (rocketVector * rocketSpeed, ForceMode2D.Force);
		} else {
			rocketRB.velocity = new Vector2 (0, 0);
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
