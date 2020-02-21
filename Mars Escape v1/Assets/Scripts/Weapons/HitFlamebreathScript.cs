using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlamebreathScript : MonoBehaviour {

	public float flamebreathDamage;
	//FlamebreathProjectileController FbrPC;
	public List<string> cosmonautNames;

	// Use this for initialization
	void Awake () {
		//FbrPC = GetComponentInParent <FlamebreathProjectileController> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Cosmonaut") {
			if (!cosmonautNames.Contains (other.name)) {
				cosmonautNames.Add (other.name);
				StartCoroutine ("DOTTimerIncrementor", other);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Cosmonaut") {
			if (cosmonautNames.Contains (other.name)) {
				cosmonautNames.Remove (other.name);
				StopCoroutine ("DOTTimerIncrementor");
			}
		}
	}

	private IEnumerator DOTTimerIncrementor(Collider2D other)
	{
		while (true) {
			yield return new WaitForSeconds (0.3f);
			if(other != null)
				other.GetComponent<cosmonautBase> ().takeDamage (flamebreathDamage);
		}
	}
}
