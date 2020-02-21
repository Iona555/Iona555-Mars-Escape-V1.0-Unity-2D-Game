using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specterController : MonoBehaviour {

	// Movement variables
	public float specterImpulseSpeed;

	// Other variables
	Animator specterAnimator;
	public bool canFlip = true;
	public bool facingRight = false;

	// Rigidbody
	Rigidbody2D specterRB;

	// State Machine
	specterStatesMachine specterSM;

	// Use this for initialization
	void Start () {
		specterAnimator = GetComponent<Animator> ();
		specterRB = GetComponent<Rigidbody2D> ();
		specterSM = GetComponent<specterStatesMachine> ();
		foreach (AnimationClip a in specterAnimator.runtimeAnimatorController.animationClips) {
			if (a.name == "SpecterAttack") {
				specterSM.DamageAndHealEffectTime = a.length * 2.1f;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		canFlip = !specterSM.LockOnTarget;
	}

	void flip () {
		if (canFlip) {
			facingRight = !facingRight;
			Vector3 specterScale = transform.localScale; //get x y z
			specterScale.x *= -1;
			transform.localScale = specterScale;
		}
	}

	public void flipTowardsTarget (GameObject Target) {
		if (facingRight && (Target.transform.position.x < transform.position.x))
			flip ();
		else if (!facingRight && (Target.transform.position.x > transform.position.x))
			flip ();
	}

	public void moveTowardsTarget (GameObject Target, bool isChaising) {
		specterAnimator.SetBool ("isChaising", isChaising);
		if (isChaising) {
			if (!facingRight)
				specterRB.AddForce (new Vector2 (-1, 0) * specterImpulseSpeed, ForceMode2D.Impulse);
			else
				specterRB.AddForce (new Vector2 (1, 0) * specterImpulseSpeed, ForceMode2D.Impulse);
		} else {
			specterRB.velocity = new Vector2 (0, 0);
		}
	}

	public void attackTarget (GameObject Target, bool isAttacking) {
		specterAnimator.SetBool ("isAttacking", isAttacking);
	}
}
