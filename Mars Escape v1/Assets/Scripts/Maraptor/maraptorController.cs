using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maraptorController : MonoBehaviour {

	// Movement variables
	public float maraptorMaxSpeed;

	// Jumping variables
	public float maraptorJumpHeight = 2000;
	bool maraptorGrounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;

	// Control variables
	public bool isControllable = true;

	// Other variables
	Rigidbody2D maraptorRB;
	Animator maraptorAnimator;
	maraptorBase maraptorP;
	bool maraptorGroundedOneUpdateBefore = false;
	public float maxSafeFallingForce;
	public float fallDamage;
	public bool facingRight;

	// The platform it sits on
	public GameObject platform;

	// Use this for initialization
	void Start () { 
		maraptorRB = GetComponent<Rigidbody2D> ();
		maraptorAnimator = GetComponent<Animator> ();
		maraptorP = this.GetComponent<maraptorBase> ();
		facingRight = true;
		fallDamage = 0;
	}

	// Update is called once per frame, every frame, which can varry
	void Update() {
		// Jumping code
		if (isControllable) {
			if (maraptorP.jumpBoost)
				maraptorJumpHeight = 300;
			else
				maraptorJumpHeight = 180;
			
			if (maraptorGrounded && Input.GetAxis("Jump") > 0 /*Input.GetKeyDown (KeyCode.Space)*/) {	// Verifies if the character is grounded & compares the vector from the jumping key (w) with 0
				maraptorGrounded = false;
				maraptorAnimator.SetBool ("maraptorOnGround", maraptorGrounded);	// Turns the jumping animation on
				maraptorRB.AddForce (new Vector2 (0, maraptorJumpHeight /**3f*/));	// Jumps the rigid body
			}
		}

		// Death animation
		if(maraptorP.isDead) {
			maraptorRB.gravityScale = 0;
			foreach (Collider2D c in GetComponents<Collider2D> ()) {
				c.enabled = false;
			}
			maraptorAnimator.SetBool ("maraptorIsDead", true);
		}
	}

	// FixedUpdate is called every physics step; it is called every 0.02 seconds
	void FixedUpdate () {
		// Falling Code
		maraptorGroundedOneUpdateBefore = maraptorGrounded; // Retains wether the cosmonaut was grounded or not 1 update before
		maraptorGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check if the character is grounded
		maraptorAnimator.SetBool ("maraptorOnGround", maraptorGrounded);	// Turns on the falling animation depending on the grounded state
		maraptorAnimator.SetFloat("maraptorVerticalSpeed", maraptorRB.velocity.y);	// Turns the landing animation on, done automatically anyway

		// Damage from falling - the fall damage is the value from the one update before
		if (!maraptorGroundedOneUpdateBefore && maraptorGrounded && fallDamage > maxSafeFallingForce)
			maraptorP.takeDamage (fallDamage);
		fallDamage = Mathf.Abs (maraptorRB.velocity.y);

		// Movement code
		if (isControllable) {
			float move = Input.GetAxis ("Horizontal");	// Gets the vector from the moving key (a or d)
			maraptorAnimator.SetFloat ("maraptorHorizontalSpeed", Mathf.Abs (move));	// Turns the running animation on
			maraptorRB.velocity = new Vector2 (move * maraptorMaxSpeed, maraptorRB.velocity.y); // Moves the rigid body

			if ((move > 0) && !facingRight) {		// move to right
				flip ();
			} else if ((move < 0) && facingRight) {	// move to left
				flip ();
			}
		}
		else
			maraptorAnimator.SetFloat ("maraptorHorizontalSpeed", 0);	// Turns the running animation off
	}

	void flip () {
		facingRight = !facingRight;
		Vector3 maraptorScale = transform.localScale; //get x y z
		maraptorScale.x *= -1;
		transform.localScale = maraptorScale;
	}

	void OnCollisionEnter2D(Collision2D newCollision) {
		Collider2D other = newCollision.collider;
		if (other.gameObject.layer == 8) { //Ground
			if (other.tag == "Small_Platform" || other.tag == "Medium_Platform" || other.tag == "Large_Platform") {
				platform = other.gameObject;
			}
		}
	}

	/*
	void OnCollisionEnter2D(Collision2D newCollision) {
		Collider2D other = newCollision.collider;
		Debug.Log ("enter");
		switch(other.tag)
		{
		case "Cosmonaut":
			if (other.GetComponent<cosmonautStateMachine> ().currentState != cosmonautStateMachine.CharacterState.CONTROLLED) {
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX |
				RigidbodyConstraints2D.FreezePositionY |
				RigidbodyConstraints2D.FreezeRotation;
			}
			break;
		case "Maraptor":
			if (other.GetComponent<maraptorStateMachine> ().currentState != maraptorStateMachine.CharacterState.CONTROLLED) {
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX |
				RigidbodyConstraints2D.FreezePositionY |
				RigidbodyConstraints2D.FreezeRotation;
			}
			break;
		}
	}

	void OnCollisionExit2D(Collision2D oldCollision) {
		Collider2D other = oldCollision.collider;
		Debug.Log ("exit");
		switch(other.tag)
		{
		case "Cosmonaut":
			if (other.GetComponent<cosmonautStateMachine> ().currentState != cosmonautStateMachine.CharacterState.CONTROLLED) {
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			break;
		case "Maraptor":
			if (other.GetComponent<maraptorStateMachine> ().currentState != maraptorStateMachine.CharacterState.CONTROLLED) {
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.None;
				other.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			}
			break;
		}
	}
	*/
}
