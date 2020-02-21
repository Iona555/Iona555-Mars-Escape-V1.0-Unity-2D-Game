using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specterCharacterDetector : MonoBehaviour {

	specterStatesMachine specterSM;
	GameObject platformSpawnedOn;
	void Start () {
		specterSM = GetComponentInParent <specterStatesMachine> ();
		if (specterSM.GSM.mostDamagedCharacter.tag == "Cosmonaut") {
			platformSpawnedOn = specterSM.GSM.mostDamagedCharacter.GetComponent<cosmonautController> ().platform;
		} else if (specterSM.GSM.mostDamagedCharacter.tag == "Maraptor") {
			platformSpawnedOn = specterSM.GSM.mostDamagedCharacter.GetComponent<maraptorController> ().platform;
		}

		BoxCollider2D myBoxCollider = GetComponent<BoxCollider2D> ();
		switch (platformSpawnedOn.tag) {
		case "Small_Platform":
			myBoxCollider.size = new Vector2(8.0f, 5.0f);
			break;
		case "Medium_Platform":
			myBoxCollider.size = new Vector2 (16.0f, 5.0f);
			break;
		case "Large_Platform":
			myBoxCollider.size = new Vector2 (27.0f, 5.0f);
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Maraptor") {
			if (!specterSM.Targets.Contains (other.gameObject)) {
				specterSM.Targets.Add (other.gameObject);
			}
		} else if (other.tag == "Cosmonaut") {
			if (!specterSM.Targets.Contains (other.gameObject)) {
				specterSM.Targets.Add (other.gameObject);
			}
		}
		Destroy (gameObject);
	}
}
