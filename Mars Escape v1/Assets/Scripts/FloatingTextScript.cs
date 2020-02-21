using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour {
	public float DestroyTime = 1.2f;
	public Vector3 Offset;
	public Vector3 CharacterScale;
	public Vector3 FloatingTextScale;

	// Use this for initialization
	void Start () {
		Offset = new Vector3 (0, 10, 0);
		transform.localPosition += Offset;
		Destroy (gameObject, DestroyTime);
	}

	void Update () {
		CharacterScale = transform.parent.transform.localScale;
		FloatingTextScale = transform.localScale;

		// Rescale text for Maraptor
		if (transform.parent.tag == "Maraptor") {
			FloatingTextScale.x = 0.5f;
			FloatingTextScale.y = 0.5f;
		}

		// Reverse the facing direction if it's the case
		if ((CharacterScale.x < 0 && FloatingTextScale.x > 0) || (CharacterScale.x > 0 && FloatingTextScale.x < 0)) {
			FloatingTextScale.x *= -1;
		}
		transform.localScale = FloatingTextScale;
	}
}