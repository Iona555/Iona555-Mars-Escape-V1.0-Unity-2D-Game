using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawnerCharacterDetector : MonoBehaviour {

	CrateSpawner myCrateSpawner;
	public List<GameObject> CharactersInRange;

	// Use this for initialization
	void Start () {
		myCrateSpawner = GetComponentInParent <CrateSpawner> ();
	}

	void Update () {
		// Decides wether there are more than one characters in range or not
		if (CharactersInRange.Count > 0)
			myCrateSpawner.CharacterTooClose = true;
		else
			myCrateSpawner.CharacterTooClose = false;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Character") && !CharactersInRange.Contains(other.gameObject)) {
			Debug.Log (other.tag + " has entered the range of " + this.transform.parent.name);
			CharactersInRange.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.layer == LayerMask.NameToLayer("Character") && CharactersInRange.Contains(other.gameObject)) {
			Debug.Log (other.tag + " has exited the range of " + this.transform.parent.name);
			CharactersInRange.Remove (other.gameObject);
		}
	}
}
