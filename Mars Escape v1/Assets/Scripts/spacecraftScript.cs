using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecraftScript : MonoBehaviour {

	// Comunicate with the GameStateMachine
	public GameStateMachine GSM;

	void Start() {
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
	}

	void OnTriggerEnter2D (Collider2D other) {
		switch (other.tag) {
		case "Cosmonaut":
			GSM.GameState = GameStateMachine.GameStateEnum.GAME_OVER;
			GSM.winners = "The Cosmonauts";
			break;
		case "Maraptor":
			GSM.GameState = GameStateMachine.GameStateEnum.GAME_OVER;
			GSM.winners = "The Maraptors";
			break;
		}
	}
}
