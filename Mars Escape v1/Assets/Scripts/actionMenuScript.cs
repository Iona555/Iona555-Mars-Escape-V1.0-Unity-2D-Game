using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class actionMenuScript : MonoBehaviour
{
	public GameObject panel;
	public bool panelVisible = false;
	public GameObject AttackButtonWrapper;
	public GameObject AttackButton;
	public GameObject WeaponUsages;
	public GameObject FlameUsages;
	public GameObject JumpBoostUsages;
	public GameObject ShieldUsages;
	public GameObject HealUsages;
	public GameObject Seconds;
	public GameObject Timer;
	public Image TimerImage;
	// Comunicate with the GameStateMachine
	public GameStateMachine GSM;

	// Current cosmonaut and maraptor properties - populated from gameStateMachine every update
	public cosmonautBase cosmonautP;
	public maraptorBase maraptorP;

	// Characters animators
	public Animator cosmonautAnimator;
	public Animator maraptorAnimator;

	// Projectiles
	public GameObject Rocket;
	public GameObject Flamethrower;
	public GameObject Fireball;
	public GameObject Flamebreath;

	// Weapon disarm timing variables
	bool isWaiting;
	bool wasWaitingDuringTheLastFrame;
	GameObject CharacterToBeDisarmed;

	void Start () {
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
		panel.SetActive (false);
		AttackButtonWrapper = transform.GetChild (0).transform.GetChild (2).gameObject;
		AttackButtonWrapper.gameObject.SetActive (false);
		AttackButton = AttackButtonWrapper.transform.GetChild (0).gameObject;
		TimerImage = Timer.GetComponent<Image> ();
		TimerImage.fillAmount = 0;
		isWaiting = false;
		wasWaitingDuringTheLastFrame = false;
	}

	// Update is called once per frame
	void Update () {
		// Update the Timer
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN || GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
			TimerImage.fillAmount = ((GSM.turnTimer + 1 > GSM.turnLenght - 2) ? 1 : ((float)(GSM.turnTimer) / (GSM.turnLenght - 2)));
			Seconds.GetComponent<Text> ().text = (((GSM.turnLenght - 2 - GSM.turnTimer) > 0) ? (GSM.turnLenght - 2 - GSM.turnTimer) : 0).ToString ();
		} else {
			TimerImage.fillAmount = 0;
			Seconds.GetComponent<Text> ().text = string.Empty;
		}

		// Show/hide the attack button
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (!AttackButtonWrapper.activeSelf && (cosmonautP.rocketLauncher || cosmonautP.flameThrower)) {
				AttackButtonWrapper.SetActive (true);
			} else if (AttackButtonWrapper.activeSelf && !(cosmonautP.rocketLauncher || cosmonautP.flameThrower)) {
				AttackButtonWrapper.SetActive (false);
			}
		} else if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
			if (!AttackButtonWrapper.activeSelf && (maraptorP.fireball || maraptorP.flameBreath)) {
				AttackButtonWrapper.SetActive (true);
			} else if (AttackButtonWrapper.activeSelf && !(maraptorP.fireball || maraptorP.flameBreath)) {
				AttackButtonWrapper.SetActive (false);
			}
		} else {
			AttackButtonWrapper.SetActive (false);
		}

		// disarming after waiting
		if (isWaiting && (wasWaitingDuringTheLastFrame == false)) {
			wasWaitingDuringTheLastFrame = true;
		} else if (!isWaiting && (wasWaitingDuringTheLastFrame == true)) {
			wasWaitingDuringTheLastFrame = false;
			if (CharacterToBeDisarmed.tag == "Cosmonaut") {
				cosmonautBase auxCosmonautP = CharacterToBeDisarmed.GetComponent<cosmonautBase> ();
				Animator auxCosmonautAnimator = CharacterToBeDisarmed.GetComponent<Animator> ();
				if (auxCosmonautP.rocketLauncher == true) {
					auxCosmonautP.rocketLauncher = false; // consumes the rocketlauncher
					auxCosmonautAnimator.SetBool ("cosmonautRocketOn", auxCosmonautP.rocketLauncher);
				} else if (auxCosmonautP.flameThrower == true) {
					auxCosmonautP.flameThrower = false; // consumes the flamethrower
					auxCosmonautAnimator.SetBool ("cosmonautFlameOn", auxCosmonautP.flameThrower);
					auxCosmonautP.transform.GetChild (0).gameObject.SetActive (auxCosmonautP.flameThrower); // turns off the range indicator
				}
			} else if (CharacterToBeDisarmed.tag == "Maraptor") {
				maraptorBase auxMaraptorP = CharacterToBeDisarmed.GetComponent<maraptorBase> ();
				Animator auxMaraptorAnimator = CharacterToBeDisarmed.GetComponent<Animator> ();
				if(auxMaraptorP.fireball == true) {
					auxMaraptorP.fireball = false; // consumes the fireball
					auxMaraptorAnimator.SetBool ("maraptorBallOn", auxMaraptorP.fireball);
				} else if (auxMaraptorP.flameBreath == true) {
					auxMaraptorP.flameBreath = false; // consumes the flamebreath
					auxMaraptorAnimator.SetBool ("maraptorFlameOn", auxMaraptorP.flameBreath);
					auxMaraptorP.transform.GetChild (0).gameObject.SetActive (auxMaraptorP.flameBreath);
				}
			}
			CharacterToBeDisarmed = null;
			AttackButton.GetComponent<Button> ().interactable = true;
		}

	}

	public void ShowAndHidePanel ()
	{
		panelVisible = !panelVisible;
		if (panelVisible) {
			panel.SetActive (true);
			Time.timeScale = 0; // pause the game
		} else {
			panel.SetActive (false);
			Time.timeScale = 1; // resume
		}
	}

	public void UseWeapon ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.rocketLauncherUsages > 0 && !cosmonautP.rocketLauncher) {
				// Equips the rocketLauncher
				cosmonautP.rocketLauncher = true;
				cosmonautP.rocketLauncherUsages--;
				cosmonautAnimator.SetBool ("cosmonautRocketOn", cosmonautP.rocketLauncher);

				if (cosmonautP.flameThrower) {
					// Unequips the flameThrower
					cosmonautP.flameThrowerUsages++;
					cosmonautP.flameThrower = false;
					cosmonautAnimator.SetBool ("cosmonautFlameOn", cosmonautP.flameThrower);
				}
			}
			else
			{
				if (cosmonautP.rocketLauncher && CharacterToBeDisarmed != cosmonautP.gameObject) {
					// Unequips the rocketLauncher
					cosmonautP.rocketLauncher = false;
					cosmonautP.rocketLauncherUsages++;
					cosmonautAnimator.SetBool ("cosmonautRocketOn", cosmonautP.flameThrower);
				}
			}
			// Switch the flame range indicator on or off
			cosmonautP.transform.GetChild (0).gameObject.SetActive (cosmonautP.flameThrower);
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.fireballUsages > 0 && !maraptorP.fireball) {
					// Equips the fireBall
					maraptorP.fireball = true;
					maraptorP.fireballUsages--;
					maraptorAnimator.SetBool ("maraptorBallOn", maraptorP.fireball);
					if (maraptorP.flameBreath) {
						// Unequips the fireBreath
						maraptorP.flameBreathUsages++;
						maraptorP.flameBreath = false;
						maraptorAnimator.SetBool ("maraptorFlameOn", maraptorP.flameBreath);
					}
				}
				else
				{
					if (maraptorP.fireball && CharacterToBeDisarmed != maraptorP.gameObject) {
						// Unequips the fireBall
						maraptorP.fireball = false;
						maraptorP.fireballUsages++;
						maraptorAnimator.SetBool ("maraptorBallOn", maraptorP.fireball);
					}
				}
				// Switch the flame range indicator on or off
				maraptorP.transform.GetChild (0).gameObject.SetActive (maraptorP.flameBreath);
			}
		}
	}

	public void UseFlame ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.flameThrowerUsages > 0 && !cosmonautP.flameThrower) {
				// Equips the flameThrower
				cosmonautP.flameThrower = true;
				cosmonautP.flameThrowerUsages--;
				cosmonautAnimator.SetBool ("cosmonautFlameOn", cosmonautP.flameThrower);
				if (cosmonautP.rocketLauncher) {
					// Unequips the rocketLauncher
					cosmonautP.rocketLauncherUsages++;
					cosmonautP.rocketLauncher = false;
					cosmonautAnimator.SetBool ("cosmonautRocketOn", cosmonautP.rocketLauncher);
				}
			}
			else
			{
				if (cosmonautP.flameThrower && CharacterToBeDisarmed != cosmonautP.gameObject) {
					// Unequips the flameThrower
					cosmonautP.flameThrower = false;
					cosmonautP.flameThrowerUsages++;
					cosmonautAnimator.SetBool ("cosmonautFlameOn", cosmonautP.flameThrower);
				}
			}
			// Switch the flame range indicator on or off
			cosmonautP.transform.GetChild (0).gameObject.SetActive (cosmonautP.flameThrower);
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.flameBreathUsages > 0 && !maraptorP.flameBreath) {
					// Equips the flameBreath
					maraptorP.flameBreath = true;
					maraptorP.flameBreathUsages--;
					maraptorAnimator.SetBool ("maraptorFlameOn", maraptorP.flameBreath);
					if (maraptorP.fireball) {
						// Unequips the flameBall
						maraptorP.fireballUsages++;
						maraptorP.fireball = false;
						maraptorAnimator.SetBool ("maraptorBallOn", maraptorP.fireball);
					}
				}
				else
				{
					if (maraptorP.flameBreath && CharacterToBeDisarmed != maraptorP.gameObject) {
						// Unequips the flameThrower
						maraptorP.flameBreath = false;
						maraptorP.flameBreathUsages++;
						maraptorAnimator.SetBool ("maraptorFlameOn", maraptorP.flameBreath);
					}
				}
				// Switch the flame range indicator on or off
				maraptorP.transform.GetChild (0).gameObject.SetActive (maraptorP.flameBreath);
			}
		}
	}

	public void UseJumpBoost ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.jumpBoostUsages > 0 && !cosmonautP.jumpBoost) {
				cosmonautP.jumpBoost = true;
				cosmonautP.jumpBoostUsages--;
				cosmonautP.transform.GetChild (4).GetComponent<JumpBoostTimerScript> ().timerStarted = true;
			}
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.jumpBoostUsages > 0 && !maraptorP.jumpBoost) {
					maraptorP.jumpBoost = true;
					maraptorP.jumpBoostUsages--;
					maraptorP.transform.GetChild (4).GetComponent<JumpBoostTimerScript> ().timerStarted = true;
				}
			}
		}
	}

	public void UseShield ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.shieldUsages > 0 && cosmonautP.currentShield < cosmonautP.baseShield) {
				cosmonautP.currentShield = cosmonautP.baseShield;
				cosmonautP.shieldUsages--;
			}
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.shieldUsages > 0 && maraptorP.currentShield < maraptorP.baseShield) {
					maraptorP.currentShield = maraptorP.baseShield;
					maraptorP.shieldUsages--;
				}
			}
		}
	}

	public void UseHeal ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.healUsages > 0 && cosmonautP.currentHP < cosmonautP.baseHP) {
				cosmonautP.currentHP = cosmonautP.baseHP;
				cosmonautP.healUsages--;
			}
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.healUsages > 0 && maraptorP.currentHP < maraptorP.baseHP) {
					maraptorP.currentHP = maraptorP.baseHP;
					maraptorP.healUsages--;
				}
			}
		}
	}

	public void Attack ()
	{
		AttackButton.GetComponent<Button> ().interactable = false;
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN) {
			if (cosmonautP.rocketLauncher) {
				if(cosmonautP.transform.GetComponent<cosmonautController> ().facingRight)
					Instantiate(Rocket, cosmonautP.transform.GetChild(1).transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
				else
					Instantiate(Rocket, cosmonautP.transform.GetChild(1).transform.position, Quaternion.Euler (new Vector3 (0, 180, 0)));
				StartCoroutine ("WaitForSecondsCoroutine", 1.5f);
			} else {
				if (cosmonautP.flameThrower) {
					if(cosmonautP.transform.GetComponent<cosmonautController> ().facingRight)
						(Instantiate(Flamethrower, cosmonautP.transform.GetChild(2).transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject).transform.parent = cosmonautP.gameObject.transform;
					else
						(Instantiate(Flamethrower, cosmonautP.transform.GetChild(2).transform.position, Quaternion.Euler (new Vector3 (0, 180, 0))) as GameObject).transform.parent = cosmonautP.gameObject.transform;
					StartCoroutine ("WaitForSecondsCoroutine", 3.29f);
				}
			}
				
		} else {
			if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN) {
				if (maraptorP.fireball) {
					if(maraptorP.transform.GetComponent<maraptorController> ().facingRight)
						Instantiate(Fireball, maraptorP.transform.GetChild(1).transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
					else
						Instantiate(Fireball, maraptorP.transform.GetChild(1).transform.position, Quaternion.Euler (new Vector3 (0, 180, 0)));
					StartCoroutine ("WaitForSecondsCoroutine", 1.5f);
				} else {
					if (maraptorP.flameBreath) {
						if(maraptorP.transform.GetComponent<maraptorController> ().facingRight)
							(Instantiate(Flamebreath, maraptorP.transform.GetChild(2).transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject).transform.parent = maraptorP.gameObject.transform;
						else
							(Instantiate(Flamebreath, maraptorP.transform.GetChild(2).transform.position, Quaternion.Euler (new Vector3 (0, 180, 0))) as GameObject).transform.parent = maraptorP.gameObject.transform;
						StartCoroutine ("WaitForSecondsCoroutine", 3.29f);
					}
				}
			}
		}
	}

	IEnumerator WaitForSecondsCoroutine(float seconds) {
		if (GSM.GameState == GameStateMachine.GameStateEnum.COSMONAUT_TURN)
			CharacterToBeDisarmed = cosmonautP.gameObject;
		else if (GSM.GameState == GameStateMachine.GameStateEnum.MARAPTOR_TURN)
			CharacterToBeDisarmed = maraptorP.gameObject;
		isWaiting = true;
		yield return new WaitForSeconds (seconds);
		isWaiting = false;
	}

	public void UpdateUsagesCosmonaut () {
		WeaponUsages.GetComponent<Text> ().text = cosmonautP.rocketLauncherUsages.ToString();
		FlameUsages.GetComponent<Text> ().text = cosmonautP.flameThrowerUsages.ToString();
		JumpBoostUsages.GetComponent<Text> ().text = cosmonautP.jumpBoostUsages.ToString();
		ShieldUsages.GetComponent<Text> ().text = cosmonautP.shieldUsages.ToString();
		HealUsages.GetComponent<Text> ().text = cosmonautP.healUsages.ToString();
	}

	public void UpdateUsagesMaraptor () {
		WeaponUsages.GetComponent<Text> ().text = maraptorP.fireballUsages.ToString();
		FlameUsages.GetComponent<Text> ().text = maraptorP.flameBreathUsages.ToString();
		JumpBoostUsages.GetComponent<Text> ().text = maraptorP.jumpBoostUsages.ToString();
		ShieldUsages.GetComponent<Text> ().text = maraptorP.shieldUsages.ToString();
		HealUsages.GetComponent<Text> ().text = maraptorP.healUsages.ToString();
	}

}