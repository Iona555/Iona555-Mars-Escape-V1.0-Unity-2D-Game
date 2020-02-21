using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlamethrowerScript : MonoBehaviour {

	public float flamethrowerDamage;
	//FlamethrowerProjectileController FthPC;
	public List<string> maraptorNames;

	// Use this for initialization
	void Awake () {
		//FthPC = GetComponentInParent <FlamethrowerProjectileController> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Maraptor") {
			if (!maraptorNames.Contains (other.name)) {
				maraptorNames.Add (other.name);
				StartCoroutine ("DOTTimerIncrementor", other);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Maraptor") {
			if (maraptorNames.Contains (other.name)) {
				maraptorNames.Remove (other.name);
				StopCoroutine ("DOTTimerIncrementor");
			}
		}
	}

	private IEnumerator DOTTimerIncrementor(Collider2D other)
	{
		while (true) {
			yield return new WaitForSeconds (0.3f);
			if(other != null)
				other.GetComponent<maraptorBase> ().takeDamage (flamethrowerDamage);
		}
	}
}
