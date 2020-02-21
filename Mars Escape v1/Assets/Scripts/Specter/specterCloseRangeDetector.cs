using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class specterCloseRangeDetector : MonoBehaviour {

	specterStatesMachine specterSM;

	void Start () {
		specterSM = GetComponentInParent <specterStatesMachine> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Maraptor" || other.tag == "Cosmonaut") {
			if (specterSM.Targets.Contains (other.gameObject) && specterSM.Target == other.gameObject) {
				specterSM.isChaising = false;
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Maraptor" || other.tag == "Cosmonaut") {
			if (specterSM.Targets.Contains (other.gameObject) && specterSM.Target == other.gameObject) {
				specterSM.isChaising = false;
			}
		}
	}
}
