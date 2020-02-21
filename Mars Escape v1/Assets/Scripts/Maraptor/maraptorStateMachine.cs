using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maraptorStateMachine : MonoBehaviour {

	public maraptorBase maraptorP;
	public GameStateMachine GSM;
	public maraptorController maraptorController;

	public enum CharacterState {
		IDLE,
		CONTROLLED,
		//SELECT,
		//PERFORM,
		DEAD
	}
	public CharacterState currentState;

	void Start () {
		maraptorP = this.GetComponent<maraptorBase> ();
		currentState = CharacterState.IDLE; // Starting state
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();	// Comunicate with the GameStateMachine
		maraptorController = GetComponent<maraptorController> (); // Comunicate with the Atached Controller
		maraptorP.currentPosition = transform.position; // Current position initialisation
	}

	void Update () {
		maraptorP.currentPosition = transform.position;
		// Check if the maraptor is dead
		if (maraptorP.isDead && currentState != CharacterState.DEAD)
			currentState = CharacterState.DEAD;
		// Maraptor State Machine
		switch (currentState) {
		case (CharacterState.IDLE):
			// Idle state
			maraptorController.isControllable = false;
			break;
		case (CharacterState.CONTROLLED):
			// Controlled state
			maraptorController.isControllable = true;
			break;
		/*
		case (CharacterState.SELECT):
			// Also works as a pause
			// Select action and target
			maraptorController.isControllable = false;
			currentState = CharacterState.PERFORM;
			break;
		case (CharacterState.PERFORM):
			// Perform selected action state
			maraptorController.isControllable = false;
			currentState = CharacterState.IDLE;
			break;
		*/
		case (CharacterState.DEAD):
			maraptorController.isControllable = false;
			break;
		}
	}
}
