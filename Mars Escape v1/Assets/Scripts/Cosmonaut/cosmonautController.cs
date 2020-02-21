using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cosmonautController : MonoBehaviour {

    // Movement variables
    public float cosmonautMaxSpeed;
	float move;

	// Jumping variables
	public float cosmonautJumpHeight = 200;
	bool cosmonautGrounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;

	// Control variables
	public bool isControllable = true;

	// Other variables
    Rigidbody2D cosmonautRB;
	public Animator cosmonautAnimator;
	cosmonautBase cosmonautP;
	bool cosmonautGroundedOneUpdateBefore = false;
	public float maxSafeFallingForce;
	public float fallDamage;
	public bool facingRight;

	// The platform it sits on
	public GameObject platform;

	// Use this for initialization
	void Start () {
		cosmonautRB = GetComponent<Rigidbody2D> ();
		cosmonautAnimator = GetComponent<Animator> ();
		cosmonautP = this.GetComponent<cosmonautBase> ();
		facingRight = true;
		fallDamage = 0;
		move = 0;
	}

	// Update is called once per frame, every frame, which can varry
	void Update() {
		// Jumping code
		if (isControllable) {
			if (cosmonautP.jumpBoost)
				cosmonautJumpHeight = 300;
			else
				cosmonautJumpHeight = 180;
			
			if (cosmonautGrounded && Input.GetAxis("Jump") > 0 /*Input.GetKeyDown (KeyCode.Space)*/) {	// Verifies if the character is grounded & compares the vector from the jumping key (space) with 0
				cosmonautGrounded = false;
				cosmonautAnimator.SetBool ("cosmonautOnGround", cosmonautGrounded);	// Turns the jumping animation on
				cosmonautRB.AddForce (new Vector2 (0, cosmonautJumpHeight /**3f*/));	// Jumps the rigid body
			}
		}

		// Death animation
		if(cosmonautP.isDead) {
			cosmonautRB.gravityScale = 0;
			foreach (Collider2D c in GetComponents<Collider2D> ()) {
				c.enabled = false;
			}
			cosmonautAnimator.SetBool ("cosmonautIsDead", true);
		}
	}

	// FixedUpdate is called every physics step; it is called every 0.02 seconds
	void FixedUpdate () {
		// Falling Code
		cosmonautGroundedOneUpdateBefore = cosmonautGrounded; // Retains wether the cosmonaut was grounded or not 1 update before
		cosmonautGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Check if the character is grounded
		cosmonautAnimator.SetBool ("cosmonautOnGround", cosmonautGrounded);	// Turns on the falling animation depending on the grounded state
		cosmonautAnimator.SetFloat("cosmonautVerticalSpeed", cosmonautRB.velocity.y);	// Turns the landing animation on, done automatically anyway

		// Damage from falling - the fall damage is the value from the one update before
		if (!cosmonautGroundedOneUpdateBefore && cosmonautGrounded && fallDamage > maxSafeFallingForce)
			cosmonautP.takeDamage (fallDamage);
		fallDamage = Mathf.Abs (cosmonautRB.velocity.y);

		// Movement code
		if (isControllable) {
			move = Input.GetAxis ("Horizontal");	// Gets the vector from the moving key (a or d)
			/*if (Input.GetKeyDown (KeyCode.A))
				move = -1;
			if (Input.GetKeyDown (KeyCode.D))
				move = 1;
			if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.D))
				move = 0;*/
			cosmonautAnimator.SetFloat ("cosmonautHorizontalSpeed", Mathf.Abs (move));	// Turns the running animation on
			cosmonautRB.velocity = new Vector2 (move * cosmonautMaxSpeed, cosmonautRB.velocity.y); // Moves the rigid body

			if ((move > 0) && !facingRight) {		// move to right
				flip ();
			} else if ((move < 0) && facingRight) {	// move to left
				flip ();
			}
		} else {
			cosmonautAnimator.SetFloat ("cosmonautHorizontalSpeed", 0);	// Turns the running animation off
		}
	}

	void flip () {
		facingRight = !facingRight;
		Vector3 cosmonautScale = transform.localScale; //get x y z
		cosmonautScale.x *= -1;
		transform.localScale = cosmonautScale;
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
