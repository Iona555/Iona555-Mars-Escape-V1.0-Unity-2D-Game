using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : MonoBehaviour {
	// Game States
	public enum GameStateEnum {
		INTRO,
		COSMONAUT_TURN,
		MARAPTOR_TURN,
		SPECTER_TURN,
		CRAWLER_TURN,
		PAUSE_GAME,
		GAME_OVER
	}
	public GameStateEnum GameState;
	public GameStateEnum previeousState;

	// Game objects for cosmonauts and maraptors
	public List<GameObject> Cosmonauts = new List<GameObject> ();
	public List<GameObject> Maraptors = new List<GameObject> ();

	// State Machines for cosmonauts and maraptors
	public List<cosmonautStateMachine> CosmonautStateMachines = new List<cosmonautStateMachine> ();
	public List<maraptorStateMachine> MaraptorStateMachines = new List<maraptorStateMachine> ();

	// Characters' properties
	cosmonautBase cosmonautP;
	maraptorBase maraptorP;

	// MainCamera
	public cameraFollowPlayer MainCameraPlayer;

	// Abilities menu
	actionMenuScript AM;

	// Game Management Variables
	public int turnTimer;
	public int turnLenght;
	public int numberOfCosmonauts = 0;
	public int numberOfMaraptors = 0;
	public int currentCosmonaut = 0;
	public int currentMaraptor = 0;

	// A.I. variables
	public GameObject Specter;
	public GameObject Crawler;
	public bool SpecterSpawned;
	public bool CrawlerSpawned;
	public GameObject mostDamagedCharacter;
	public bool SpecterIsDone;
	public bool CrawlerIsDone;

	// First turn checker
	int numberOfPlayerCharacters;
	int numberOfPlayedOnCharacters;
	public bool firstTurn;

	// Pause related object
	public GameObject  pauseObject;

	// Winners
	public string winners = "none";

	// Initialization
	void Start () {
		// Set the turnLenght
		turnLenght = 18;

		// Set the first turn to true
		firstTurn = true;

		// Populate the game objects for cosmonauts and maraptors
		Cosmonauts.AddRange (GameObject.FindGameObjectsWithTag ("Cosmonaut"));
		Maraptors.AddRange (GameObject.FindGameObjectsWithTag ("Maraptor"));

		// Populate the State Machines for cosmonauts and maraptors, editing their names and startup properites
		foreach (GameObject Cosmonaut in Cosmonauts) {
			cosmonautP = Cosmonaut.GetComponent<cosmonautBase> ();
			cosmonautP.name = Cosmonaut.transform.gameObject.name;
			cosmonautP.baseHP = 100;
			cosmonautP.currentHP = 100;
			cosmonautP.baseShield = 50;
			CosmonautStateMachines.Add(Cosmonaut.GetComponent<cosmonautStateMachine> ());
		}
		foreach (GameObject Maraptor in Maraptors) {
			maraptorP = Maraptor.GetComponent<maraptorBase> ();
			maraptorP.name = Maraptor.transform.gameObject.name;
			maraptorP.baseHP = 100;
			maraptorP.currentHP = 100;
			maraptorP.baseShield = 50;
			MaraptorStateMachines.Add (Maraptor.GetComponent<maraptorStateMachine> ());
		}

		// Get the numberOfCosmonauts and numberOfMaraptors
		numberOfCosmonauts = Cosmonauts.Count;
		numberOfMaraptors = Maraptors.Count;

		// Set the number of player characters and the number of played on characters
		numberOfPlayerCharacters = numberOfCosmonauts + numberOfMaraptors;
		numberOfPlayedOnCharacters = 0;

		// Get the MainCameraPlayer
		MainCameraPlayer = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<cameraFollowPlayer> ();;

		// Get the Action Menu
		AM = GameObject.Find ("ActionMenu").GetComponent<actionMenuScript>();

		// Start Game
		GameState = GameStateEnum.INTRO;
		turnTimer = 0;
		StartCoroutine("TurnTimerIncrementor");
	}
	
	// Update is called once per frame
	void Update () {
		// Check if the game is paused
		if (pauseObject.GetComponent<pauseScript> ().gamePaused && GameState != GameStateEnum.PAUSE_GAME) {
			previeousState = GameState;
			GameState = GameStateEnum.PAUSE_GAME;
		}

		// Check either first turn is over or not
		if (firstTurn && (numberOfPlayedOnCharacters == numberOfPlayerCharacters)) {
			firstTurn = false;
		}

		// Update the action bar for the current Cosmonaut
		AM.cosmonautP = Cosmonauts [currentCosmonaut].GetComponent<cosmonautBase> ();
		AM.cosmonautAnimator = Cosmonauts [currentCosmonaut].GetComponent<Animator> ();

		// Update the action bar for the current Maraptor
		AM.maraptorP = Maraptors [currentMaraptor].GetComponent<maraptorBase> ();
		AM.maraptorAnimator = Maraptors [currentMaraptor].GetComponent<Animator> ();

		// Check if both characters of a team are dead in any moment of time
		if (RefreshCheckCosmonaut (false) || RefreshCheckMaraptor (false))
			GameState = GameStateEnum.GAME_OVER;

		// Game State Machine
		switch (GameState) {
		case (GameStateEnum.INTRO):
			if (turnTimer == 2) {
				turnTimer = 0;
				previeousState = GameStateEnum.INTRO;
				GameState = GameStateEnum.COSMONAUT_TURN;
			}
			break;

		case (GameStateEnum.COSMONAUT_TURN):
			// Turn the controller on for the current Cosmonaut if it's not dead
			if (turnTimer < (turnLenght - 2)) {
				if (CosmonautStateMachines [currentCosmonaut].currentState != cosmonautStateMachine.CharacterState.DEAD) {
					if (CosmonautStateMachines [currentCosmonaut].currentState != cosmonautStateMachine.CharacterState.CONTROLLED) {
						CosmonautStateMachines [currentCosmonaut].currentState = cosmonautStateMachine.CharacterState.CONTROLLED;
					}
				} else {
					turnTimer = turnLenght;
				}
			}
			// Set the camera on the current Cosmonaut
			MainCameraPlayer.target = Cosmonauts [currentCosmonaut].transform;

			// Udate usages for the Cosmonaut in the ActionMenu
			AM.UpdateUsagesCosmonaut ();

			if (turnTimer == (turnLenght - 3)) {
				// Turn the controller off for the current Cosmonaut 2 seconds before the turn ends
				if (CosmonautStateMachines [currentCosmonaut].currentState != cosmonautStateMachine.CharacterState.DEAD)
					CosmonautStateMachines [currentCosmonaut].currentState = cosmonautStateMachine.CharacterState.IDLE;
			}

			// End Cosmonaut turn
			// turnTimer reset on turnLenght seconds for Cosmonauts
			if ((turnTimer == turnLenght)) {
				turnTimer = 0;
				previeousState = GameStateEnum.COSMONAUT_TURN;
				++numberOfPlayedOnCharacters;

				// Decide(true) or just check(false) the next Cosmonaut and whether the game is over or not
				if (RefreshCheckCosmonaut (true))
					GameState = GameStateEnum.GAME_OVER;
				else
					GameState = GameStateEnum.SPECTER_TURN;
			}
			break;

		case (GameStateEnum.MARAPTOR_TURN):
			// Turn the controller on for the current Maraptor if it's not dead
			if (turnTimer < (turnLenght - 3)) {
				if (MaraptorStateMachines [currentMaraptor].currentState != maraptorStateMachine.CharacterState.DEAD) {
					if (MaraptorStateMachines [currentMaraptor].currentState != maraptorStateMachine.CharacterState.CONTROLLED) {
						MaraptorStateMachines [currentMaraptor].currentState = maraptorStateMachine.CharacterState.CONTROLLED;
					}
				} else {
					turnTimer = turnLenght;
				}
			}
			// Set the camera on the current Maraptor
			MainCameraPlayer.target = Maraptors [currentMaraptor].transform;

			// Udate usages for the Maraptor in the ActionMenu
			AM.UpdateUsagesMaraptor ();

			if (turnTimer == (turnLenght - 2)) {
				// Turn the controller off for the current Maraptor 2 seconds before the turn ends
				if (MaraptorStateMachines [currentMaraptor].currentState != maraptorStateMachine.CharacterState.DEAD)
					MaraptorStateMachines [currentMaraptor].currentState = maraptorStateMachine.CharacterState.IDLE;
			}

			// End Maraptor turn
			// turnTimer reset on turnLenght seconds for Maraptors
			if ((turnTimer == turnLenght)) {
				turnTimer = 0;
				previeousState = GameStateEnum.MARAPTOR_TURN;
				++numberOfPlayedOnCharacters;

				// Decide(true) or just check(false) the next Maraptor and whether the game is over or not
				if (RefreshCheckMaraptor (true))
					GameState = GameStateEnum.GAME_OVER;
				else
					GameState = GameStateEnum.CRAWLER_TURN;
			}
			break;

		case (GameStateEnum.SPECTER_TURN):
			// Spawn the specter
			if (!SpecterSpawned) {
				SpecterIsDone = false;
				DetectMostDamagedCharacter ();
				GameObject currentSpecter = null;
				Vector3 SpecterSpawnpointOffset = new Vector3 (0f, 2f, 0f);
				if (mostDamagedCharacter.tag == "Cosmonaut") {
					currentSpecter = Instantiate(Specter, mostDamagedCharacter.GetComponent<cosmonautController> ().platform.transform.position + SpecterSpawnpointOffset, Quaternion.Euler (new Vector3 (0, 0, 0)));
				} else if (mostDamagedCharacter.tag == "Maraptor") {
					currentSpecter = Instantiate(Specter, mostDamagedCharacter.GetComponent<maraptorController> ().platform.transform.position + SpecterSpawnpointOffset, Quaternion.Euler (new Vector3 (0, 0, 0)));
				}
				MainCameraPlayer.target = currentSpecter.transform;
				SpecterSpawned = true;
			}
			// turnTimer reset on 6 seconds for the Specter
			if (turnTimer == 6 || SpecterIsDone) {
				MainCameraPlayer.target = null;
				SpecterSpawned = false;
				turnTimer = 0;
				previeousState = GameStateEnum.SPECTER_TURN;
				GameState = GameStateEnum.MARAPTOR_TURN;
			}
			break;

		case (GameStateEnum.CRAWLER_TURN):
			// Spawn the crawler
			if (!CrawlerSpawned) {
				CrawlerIsDone = false;
				DetectMostDamagedCharacter ();
				GameObject currentCrawler = null;
				Vector3 CrawlerSpawnpointOffset = new Vector3 (0f, 2f, 0f);
				if (mostDamagedCharacter.tag == "Cosmonaut") {
					currentCrawler = Instantiate(Crawler, mostDamagedCharacter.GetComponent<cosmonautController> ().platform.transform.position + CrawlerSpawnpointOffset, Quaternion.Euler (new Vector3 (0, 0, 0)));
				} else if (mostDamagedCharacter.tag == "Maraptor") {
					currentCrawler = Instantiate(Crawler, mostDamagedCharacter.GetComponent<maraptorController> ().platform.transform.position + CrawlerSpawnpointOffset, Quaternion.Euler (new Vector3 (0, 0, 0)));
				}
				MainCameraPlayer.target = currentCrawler.transform;
				CrawlerSpawned = true;
			}
			// turnTimer reset on 6 seconds for the Crawler
			if (turnTimer == 6 || CrawlerIsDone) {
				MainCameraPlayer.target = null;
				CrawlerSpawned = false;
				turnTimer = 0;
				previeousState = GameStateEnum.CRAWLER_TURN;
				GameState = GameStateEnum.COSMONAUT_TURN;
			}
			break;

		case (GameStateEnum.PAUSE_GAME):
			// turnTimer doesn't reset
			if (!pauseObject.GetComponent<pauseScript> ().gamePaused) {
				GameStateEnum auxState = previeousState;
				previeousState = GameState;
				GameState = auxState;
			}

			break;
		case (GameStateEnum.GAME_OVER):

			// Game Over
			break;
		}
	}

	private IEnumerator TurnTimerIncrementor()
	{
		while (true) {
			yield return new WaitForSeconds (1);
			turnTimer += 1;
		}
	}

	public bool RefreshCheckCosmonaut (bool returnNextCosmonaut) {
		bool GameOver = false;
		// Check if at least one Cosmonaut is alive
		int cosmonautIterator = currentCosmonaut;
		while (true) {
			// Iterate through the list
			if ((cosmonautIterator + 1) < numberOfCosmonauts)	
				++cosmonautIterator;
			else
				cosmonautIterator = 0;
			// Check wether the iterated Cosmonaut is alive or dead
			if (CosmonautStateMachines [cosmonautIterator].currentState != cosmonautStateMachine.CharacterState.DEAD) {
				if (returnNextCosmonaut) {
					currentCosmonaut = cosmonautIterator;
				}
				if(CosmonautStateMachines [currentCosmonaut].currentState == cosmonautStateMachine.CharacterState.DEAD)
				{
					currentCosmonaut = cosmonautIterator;
					if(GameState == GameStateEnum.COSMONAUT_TURN)
						turnTimer = turnLenght;
				}
				break;
			} else {
				if (cosmonautIterator == currentCosmonaut) {
					winners = "The Maraptors";
					GameOver = true;
					break;
				}
			}
		}
		return GameOver;
	}

	public bool RefreshCheckMaraptor (bool returnNextMaraptor) {
		bool GameOver = false;
		// Check if at least one Cosmonaut is alive
		int maraptorIterator = currentMaraptor;
		while (true) {
			// Iterate through the list
			if ((maraptorIterator + 1) < numberOfMaraptors)	
				++maraptorIterator;
			else
				maraptorIterator = 0;
			// Check wether the iterated Cosmonaut is alive or dead
			if (MaraptorStateMachines [maraptorIterator].currentState != maraptorStateMachine.CharacterState.DEAD) {
				if (returnNextMaraptor) {
					currentMaraptor = maraptorIterator;
				}
				if(MaraptorStateMachines [currentMaraptor].currentState == maraptorStateMachine.CharacterState.DEAD) {
					currentMaraptor = maraptorIterator;
					if(GameState == GameStateEnum.MARAPTOR_TURN)
						turnTimer = turnLenght;
				}
				break;
			} else {
				if (maraptorIterator == currentMaraptor) {
					winners = "The Cosmonauts";
					GameOver = true;
					break;
				}
			}
		}
		return GameOver;
	}

	public void DetectMostDamagedCharacter () {
		// Get all the characters in a list
		List<GameObject> AliveCharacters = new List<GameObject>();

		foreach (GameObject cosmonaut in Cosmonauts) {
			if (cosmonaut.GetComponent<cosmonautStateMachine> ().currentState != cosmonautStateMachine.CharacterState.DEAD)
				AliveCharacters.Add (cosmonaut);
		}
		foreach (GameObject maraptor in Maraptors) {
			if (maraptor.GetComponent<maraptorStateMachine> ().currentState != maraptorStateMachine.CharacterState.DEAD)
				AliveCharacters.Add (maraptor);
		}

		/*
		AliveCharacters.AddRange (Cosmonauts);
		AliveCharacters.AddRange (Maraptors);

		// Remove all the dead characters from the list
		for (int characterIterator = 0; characterIterator < AliveCharacters.Count; ++characterIterator) {
			if (AliveCharacters[characterIterator].tag == "Cosmonaut") {
				if (AliveCharacters [characterIterator].GetComponent<cosmonautStateMachine> ().currentState == cosmonautStateMachine.CharacterState.DEAD) {
					AliveCharacters.Remove (AliveCharacters [characterIterator]);
					characterIterator--;
				}
			} else if (AliveCharacters[characterIterator].tag == "Maraptors") {
				if (AliveCharacters [characterIterator].GetComponent<maraptorStateMachine> ().currentState == maraptorStateMachine.CharacterState.DEAD) {
					AliveCharacters.Remove (AliveCharacters [characterIterator]);
					characterIterator--;
				}
			}
		}*/

		// Get the lowest HP in the scene
		// Get all the most damaged characters (with same HP) from the list to another list
		float minHPFound = 100;
		GameObject currentCharacter;
		float currentCharacterHP;
		List<GameObject> LowHpCharacters = new List<GameObject>();
		LowHpCharacters.Add (AliveCharacters [0]);
		if (AliveCharacters [0].tag == "Cosmonaut") {
			minHPFound = AliveCharacters [0].GetComponent<cosmonautBase> ().currentHP;
		} else if (AliveCharacters [0].tag == "Maraptor") {
			minHPFound = AliveCharacters [0].GetComponent<maraptorBase> ().currentHP;
		}
		for (int characterIterator = 1; characterIterator < AliveCharacters.Count; ++characterIterator) {
			currentCharacter = AliveCharacters [characterIterator];
			if (currentCharacter.tag == "Cosmonaut") {
				currentCharacterHP = currentCharacter.GetComponent<cosmonautBase> ().currentHP;
				if (currentCharacterHP < minHPFound) {
					minHPFound = currentCharacterHP;
					LowHpCharacters.Clear ();
					LowHpCharacters.Add (currentCharacter);
				} else if (currentCharacterHP == minHPFound) {
					LowHpCharacters.Add (currentCharacter);
				}
			} else if (currentCharacter.tag == "Maraptor") {
				currentCharacterHP = currentCharacter.GetComponent<maraptorBase> ().currentHP;
				if (currentCharacterHP < minHPFound) {
					minHPFound = currentCharacterHP;
					LowHpCharacters.Clear ();
					LowHpCharacters.Add (currentCharacter);
				} else if (currentCharacterHP == minHPFound) {
					LowHpCharacters.Add (currentCharacter);
				}
			}
		}

		// Chooses a random character from the list of low hp characters
		mostDamagedCharacter = LowHpCharacters [Random.Range (0, LowHpCharacters.Count)];

		// Empty both lists
		AliveCharacters.Clear ();
		LowHpCharacters.Clear ();
	}
}
