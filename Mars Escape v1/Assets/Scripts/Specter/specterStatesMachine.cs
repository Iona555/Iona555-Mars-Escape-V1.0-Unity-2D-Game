using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specterStatesMachine : MonoBehaviour {

	public specterController specterC;
	public GameStateMachine GSM;
	public enum CharacterState {
		EXIT_STEALTH,	// idle
		CHOOSE_TARGET, 	// idle
		MOVE_TO_TARGET, // run
		DAMAGE_TARGET,	// attack
		HEAL_TARGET,	// idle
		ENTER_STEALTH	// idle
	}
	public CharacterState currentState;

	// Behaviour variables
	public float InvisibilityEffectTime;
	public float DamageAndHealEffectTime;
	public float SpecterDamage;
	public float SpecterHeal;

	// Targetting variables
	public List<GameObject> Targets;
	int currentTargetIterator;
	public GameObject Target;
	public bool LockOnTarget;
	public bool isChaising;
	public bool isWaiting;
	public bool WaitForSecondsCoroutineRunning;

	// Use this for initialization
	void Start () {
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
		specterC = GetComponent<specterController> ();
		currentTargetIterator = 0;
		currentState = CharacterState.EXIT_STEALTH;
		LockOnTarget = false;
		isChaising = false;
		isWaiting = false;
		WaitForSecondsCoroutineRunning = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Specter State Machine
		switch (currentState) {
		case (CharacterState.EXIT_STEALTH):
			// Wait for a few seconds
			// Switch to CHOOSE_TARGET
			if(!WaitForSecondsCoroutineRunning)
				StartCoroutine ("WaitForSecondsCoroutine", InvisibilityEffectTime);
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				if (Targets.Count != 0) {
					currentState = CharacterState.CHOOSE_TARGET;
				} else {
					currentState = CharacterState.ENTER_STEALTH;
				}
			}
			break;
		case (CharacterState.CHOOSE_TARGET):
			if (currentTargetIterator < Targets.Count) {
				Target = Targets [currentTargetIterator];
				currentTargetIterator++;
				specterC.flipTowardsTarget (Target.gameObject);
				LockOnTarget = true;
				isChaising = true;
				currentState = CharacterState.MOVE_TO_TARGET;
			} else {
				currentState = CharacterState.ENTER_STEALTH;
			}
			break;
		case (CharacterState.MOVE_TO_TARGET):
			specterC.moveTowardsTarget (Target.gameObject, isChaising);
			if (isChaising == false) {
				if (Target.tag == "Maraptor") {
					currentState = CharacterState.DAMAGE_TARGET;
				} else if (Target.tag == "Cosmonaut") {
					currentState = CharacterState.HEAL_TARGET;
				}
			}
			break;
		case (CharacterState.DAMAGE_TARGET):
			if (!WaitForSecondsCoroutineRunning) {
				specterC.attackTarget (Target.gameObject, true);
				StartCoroutine ("WaitForSecondsCoroutine", DamageAndHealEffectTime);
			}
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				specterC.attackTarget (Target.gameObject, false);
				Target.GetComponent<maraptorBase> ().takeDamage (SpecterDamage);
				LockOnTarget = false;
				currentState = CharacterState.CHOOSE_TARGET;
			}
			break;
		case (CharacterState.HEAL_TARGET):
			if (!WaitForSecondsCoroutineRunning) {
				StartCoroutine ("WaitForSecondsCoroutine", DamageAndHealEffectTime);
			}
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				Target.GetComponent<cosmonautBase> ().takeHeal (SpecterHeal);
				LockOnTarget = false;
				currentState = CharacterState.CHOOSE_TARGET;
			}
			break;
		case (CharacterState.ENTER_STEALTH):
			// Wait for a few seconds
			// Destroy Character
			if(!WaitForSecondsCoroutineRunning)
				StartCoroutine ("WaitForSecondsCoroutine", InvisibilityEffectTime);
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				// the Specter turn is over
				GSM.SpecterIsDone = true;
				Destroy (gameObject);
			}
			break;
		}
	}

	IEnumerator WaitForSecondsCoroutine(float seconds) {
		WaitForSecondsCoroutineRunning = true;
		isWaiting = true;
		yield return new WaitForSeconds (seconds);
		isWaiting = false;
	}
}
