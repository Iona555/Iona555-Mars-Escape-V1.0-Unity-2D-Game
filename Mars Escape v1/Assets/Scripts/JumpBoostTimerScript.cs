using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpBoostTimerScript : MonoBehaviour {

	public Vector3 newLocalScale;
	public Vector3 newLocalPosition;
	public GameObject parentObject;
	public string parentObjectTag;
	public Vector3 CharacterScale;

	// Timer variables
	public float JumpBoostDuration;
	public float JumpBoostTimer;
	public bool timerStarted;

	// Parent
	cosmonautBase cosmonautP;
	maraptorBase maraptorP;

	// Use this for initialization
	void Start () {
		newLocalScale = transform.localScale;
		newLocalPosition = transform.localPosition;
		parentObject = this.transform.parent.gameObject;
		parentObjectTag = parentObject.tag;
		JumpBoostDuration = 6;
		JumpBoostTimer = 0;
		if (parentObject.tag == "Cosmonaut")
			cosmonautP = parentObject.GetComponent<cosmonautBase> ();
		else if (parentObject.tag == "Maraptor")
			maraptorP = parentObject.GetComponent<maraptorBase> ();
	}
	
	// Update is called once per frame
	void Update () {
		CharacterScale = parentObject.transform.localScale;

		if ((CharacterScale.x < 0 && newLocalScale.x > 0) || (CharacterScale.x > 0 && newLocalScale.x < 0))
			newLocalScale.x *= -1;

		if((newLocalScale.x < 0 && newLocalPosition.x < 0) || (newLocalScale.x > 0 && newLocalPosition.x > 0))
			newLocalPosition.x *= -1;

		transform.localScale = newLocalScale;
		transform.localPosition = newLocalPosition;

		if (timerStarted) {
			StartCoroutine ("TimerDecrementor");
			timerStarted = false;
		}

		if (parentObject.tag == "Cosmonaut")
			newLocalScale.x = (float)(JumpBoostTimer/JumpBoostDuration);
		else if (parentObject.tag == "Maraptor")
			newLocalScale.x = (float)(0.5f)*(JumpBoostTimer/JumpBoostDuration);
	}

	IEnumerator TimerDecrementor() {
		JumpBoostTimer = JumpBoostDuration;
		while (JumpBoostTimer > 0) {
			yield return new WaitForSeconds (0.1f);
			JumpBoostTimer -= 0.1f;
		}
		JumpBoostTimer = 0;
		if (parentObject.tag == "Cosmonaut")
			cosmonautP.jumpBoost = false;
		else if (parentObject.tag == "Maraptor")
			maraptorP.jumpBoost = false;
	}
}
