using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crawlerCloseRangeDetector : MonoBehaviour {

	crawlerStatesMachine crawlerSM;

	void Start () {
		crawlerSM = GetComponentInParent <crawlerStatesMachine> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Maraptor" || other.tag == "Cosmonaut") {
			if (crawlerSM.Targets.Contains (other.gameObject) && crawlerSM.Target == other.gameObject) {
				crawlerSM.isChaising = false;
			}
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Maraptor" || other.tag == "Cosmonaut") {
			if (crawlerSM.Targets.Contains (other.gameObject) && crawlerSM.Target == other.gameObject) {
				crawlerSM.isChaising = false;
			}
		}
	}
}
