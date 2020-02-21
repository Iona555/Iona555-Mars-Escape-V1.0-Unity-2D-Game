using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cosmonautStateMachine : MonoBehaviour {

	public cosmonautBase cosmonautP;
	public GameStateMachine GSM;
	public cosmonautController cosmonautController;

	public enum CharacterState {
		IDLE,
		CONTROLLED,
		//SELECT,
		//PERFORM,
		DEAD
	}
	public CharacterState currentState;

	void Start () {
		cosmonautP = this.GetComponent<cosmonautBase> ();
		currentState = CharacterState.IDLE; // Starting state
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();	// Comunicate with the GameStateMachine
		cosmonautController = GetComponent<cosmonautController> (); // Comunicate with the Atached Controller
		cosmonautP.currentPosition = transform.position; // Current position initialisation
	}

	void Update () {
		cosmonautP.currentPosition = transform.position;
		// Check if the cosmonaut is dead
		if (cosmonautP.isDead && currentState != CharacterState.DEAD)
			currentState = CharacterState.DEAD;
		// Cosmonaut State Machine
		switch (currentState) {
		case (CharacterState.IDLE):
			// Idle state
			cosmonautController.isControllable = false;
			break;
		case (CharacterState.CONTROLLED):
			// Controlled state
			cosmonautController.isControllable = true;
			break;
		/*
		case (CharacterState.SELECT):
			// Also works as a pause
			// Select action and target
			cosmonautController.isControllable = false;
			currentState = CharacterState.PERFORM;
			break;
		case (CharacterState.PERFORM):
			// Perform selected action state
			cosmonautController.isControllable = false;
			currentState = CharacterState.IDLE;
			break;
		*/
		case (CharacterState.DEAD):
			cosmonautController.isControllable = false;
			break;
		}
	}
}
