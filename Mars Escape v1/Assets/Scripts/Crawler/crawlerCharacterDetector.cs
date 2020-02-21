using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerCharacterDetector : MonoBehaviour {

	crawlerStatesMachine crawlerSM;
	GameObject platformSpawnedOn;
	void Start () {
		crawlerSM = GetComponentInParent <crawlerStatesMachine> ();
		if (crawlerSM.GSM.mostDamagedCharacter.tag == "Cosmonaut") {
			platformSpawnedOn = crawlerSM.GSM.mostDamagedCharacter.GetComponent<cosmonautController> ().platform;
		} else if (crawlerSM.GSM.mostDamagedCharacter.tag == "Maraptor") {
			platformSpawnedOn = crawlerSM.GSM.mostDamagedCharacter.GetComponent<maraptorController> ().platform;
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
			if (!crawlerSM.Targets.Contains (other.gameObject)) {
				crawlerSM.Targets.Add (other.gameObject);
			}
		} else if (other.tag == "Cosmonaut") {
			if (!crawlerSM.Targets.Contains (other.gameObject)) {
				crawlerSM.Targets.Add (other.gameObject);
			}
		}
		Destroy (gameObject);
	}
}
