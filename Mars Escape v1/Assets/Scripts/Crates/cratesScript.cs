using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cratesScript : MonoBehaviour {

	bool taken = false;
	void OnCollisionEnter2D(Collision2D newCollision) {
		Collider2D other = newCollision.collider;
		if (!taken) {
			switch (this.tag) {
			case "Crate_Health":
				if (other.tag == "Cosmonaut") {
					other.transform.gameObject.GetComponent <cosmonautBase> ().healUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <cosmonautBase> ().currentHP =
					//	other.transform.gameObject.GetComponent <cosmonautBase> ().baseHP;
					Destroy (gameObject);
				} else if (other.tag == "Maraptor") {
					other.transform.gameObject.GetComponent <maraptorBase> ().healUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <maraptorBase> ().currentHP =
					//	other.transform.gameObject.GetComponent <maraptorBase> ().baseHP;
					Destroy (gameObject);
				} else if (other.tag == "Specter" || other.tag == "Crawler") {
					taken = true;
					Destroy (gameObject);
				}
				break;
			case "Crate_JumpBoost":
				if (other.tag == "Cosmonaut") {
					other.transform.gameObject.GetComponent <cosmonautBase> ().jumpBoostUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <cosmonautBase> ().jumpBoost = true;
					Destroy (gameObject);
				} else if (other.tag == "Maraptor") {
					other.transform.gameObject.GetComponent <maraptorBase> ().jumpBoostUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <maraptorBase> ().jumpBoost = true;
					Destroy (gameObject);
				} else if (other.tag == "Specter" || other.tag == "Crawler") {
					taken = true;
					Destroy (gameObject);
				}
				break;
			case "Crate_Shield":
				if (other.tag == "Cosmonaut") {
					other.transform.gameObject.GetComponent <cosmonautBase> ().shieldUsages++;
					//other.transform.gameObject.GetComponent <cosmonautBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Maraptor") {
					other.transform.gameObject.GetComponent <maraptorBase> ().shieldUsages++;
					//other.transform.gameObject.GetComponent <maraptorBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Specter" || other.tag == "Crawler") {
					taken = true;
					Destroy (gameObject);
				}
				break;
			case "Crate_Flame":
				if (other.tag == "Cosmonaut") {
					other.transform.gameObject.GetComponent <cosmonautBase> ().flameThrowerUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <cosmonautBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Maraptor") {
					other.transform.gameObject.GetComponent <maraptorBase> ().flameBreathUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <maraptorBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Specter" || other.tag == "Crawler") {
					taken = true;
					Destroy (gameObject);
				}
				break;
			case "Crate_Explosion":
				if (other.tag == "Cosmonaut") {
					other.transform.gameObject.GetComponent <cosmonautBase> ().rocketLauncherUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <cosmonautBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Maraptor") {
					other.transform.gameObject.GetComponent <maraptorBase> ().fireballUsages++;
					taken = true;
					//other.transform.gameObject.GetComponent <maraptorBase> ().currentShield = 50;
					Destroy (gameObject);
				} else if (other.tag == "Specter" || other.tag == "Crawler") {
					taken = true;
					Destroy (gameObject);
				}
				break;
			}
		}
	}

}