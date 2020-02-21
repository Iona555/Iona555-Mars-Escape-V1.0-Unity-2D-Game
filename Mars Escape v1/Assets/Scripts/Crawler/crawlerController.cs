using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerController : MonoBehaviour {

	// Movement variables
	public float crawlerImpulseSpeed;

	// Other variables
	Animator crawlerAnimator;
	public bool canFlip = true;
	public bool facingRight = false;
	public bool isBurrowing = false;

	// Rigidbody
	Rigidbody2D crawlerRB;

	// State Machine
	crawlerStatesMachine crawlerSM;

	// Use this for initialization
	void Start () {
		crawlerAnimator = GetComponent<Animator> ();
		crawlerRB = GetComponent<Rigidbody2D> ();
		crawlerSM = GetComponent<crawlerStatesMachine> ();
		foreach (AnimationClip a in crawlerAnimator.runtimeAnimatorController.animationClips) {
			if (a.name == "crawlerAttack") {
				crawlerSM.DamageAndHealEffectTime = a.length * 2.1f;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		canFlip = !crawlerSM.LockOnTarget;
		crawlerAnimator.SetBool ("isBurrowing", isBurrowing);
	}

	void flip () {
		if (canFlip) {
			facingRight = !facingRight;
			Vector3 crawlerScale = transform.localScale; //get x y z
			crawlerScale.x *= -1;
			transform.localScale = crawlerScale;
		}
	}

	public void flipTowardsTarget (GameObject Target) {
		if (facingRight && (Target.transform.position.x < transform.position.x))
			flip ();
		else if (!facingRight && (Target.transform.position.x > transform.position.x))
			flip ();
	}

	public void moveTowardsTarget (GameObject Target, bool isChaising) {
		crawlerAnimator.SetBool ("isChaising", isChaising);
		if (isChaising) {
			if (!facingRight)
				crawlerRB.AddForce (new Vector2 (-1, 0) * crawlerImpulseSpeed, ForceMode2D.Impulse);
			else
				crawlerRB.AddForce (new Vector2 (1, 0) * crawlerImpulseSpeed, ForceMode2D.Impulse);
		} else {
			crawlerRB.velocity = new Vector2 (0, 0);
		}
	}

	public void attackTarget (GameObject Target, bool isAttacking) {
		crawlerAnimator.SetBool ("isAttacking", isAttacking);
	}
}
