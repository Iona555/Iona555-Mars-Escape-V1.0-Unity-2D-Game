using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerStatesMachine : MonoBehaviour {

	public crawlerController crawlerC;
	public GameStateMachine GSM;
	public enum CharacterState {
		UNBURROW,	// unburrow
		CHOOSE_TARGET, 	// idle
		MOVE_TO_TARGET, // run
		DAMAGE_TARGET,	// attack
		HEAL_TARGET,	// idle
		BURROW	// burrow
	}
	public CharacterState currentState;

	// Behaviour variables
	public float UbBEffectTime;
	public float DamageAndHealEffectTime;
	public float CrawlerDamage;
	public float CrawlerHeal;

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
		crawlerC = GetComponent<crawlerController> ();
		currentTargetIterator = 0;
		currentState = CharacterState.UNBURROW;
		LockOnTarget = false;
		isChaising = false;
		isWaiting = false;
		WaitForSecondsCoroutineRunning = false;
	}

	// Update is called once per frame
	void Update () {
		// Crawler State Machine
		switch (currentState) {
		case (CharacterState.UNBURROW):
			// Wait for a few seconds
			// Switch to CHOOSE_TARGET
			if(!WaitForSecondsCoroutineRunning)
				StartCoroutine ("WaitForSecondsCoroutine", UbBEffectTime);
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				if (Targets.Count != 0) {
					currentState = CharacterState.CHOOSE_TARGET;
				} else {
					currentState = CharacterState.BURROW;
				}
			}
			break;
		case (CharacterState.CHOOSE_TARGET):
			if (currentTargetIterator < Targets.Count) {
				Target = Targets [currentTargetIterator];
				currentTargetIterator++;
				crawlerC.flipTowardsTarget (Target.gameObject);
				LockOnTarget = true;
				isChaising = true;
				currentState = CharacterState.MOVE_TO_TARGET;
			} else {
				currentState = CharacterState.BURROW;
			}
			break;
		case (CharacterState.MOVE_TO_TARGET):
			crawlerC.moveTowardsTarget (Target.gameObject, isChaising);
			if (isChaising == false) {
				if (Target.tag == "Cosmonaut") {
					currentState = CharacterState.DAMAGE_TARGET;
				} else if (Target.tag == "Maraptor") {
					currentState = CharacterState.HEAL_TARGET;
				}
			}
			break;
		case (CharacterState.DAMAGE_TARGET):
			if (!WaitForSecondsCoroutineRunning) {
				crawlerC.attackTarget (Target.gameObject, true);
				StartCoroutine ("WaitForSecondsCoroutine", DamageAndHealEffectTime);
			}
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				crawlerC.attackTarget (Target.gameObject, false);
				Target.GetComponent<cosmonautBase> ().takeDamage (CrawlerDamage);
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
				Target.GetComponent<maraptorBase> ().takeHeal (CrawlerHeal);
				LockOnTarget = false;
				currentState = CharacterState.CHOOSE_TARGET;
			}
			break;
		case (CharacterState.BURROW):
			// Wait for a few seconds
			// Destroy Character
			if (!WaitForSecondsCoroutineRunning) {
				StartCoroutine ("WaitForSecondsCoroutine", UbBEffectTime);
				crawlerC.isBurrowing = true;
			}
			if (!isWaiting) {
				WaitForSecondsCoroutineRunning = false;
				// the Crawler turn is over
				GSM.CrawlerIsDone = true;
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
