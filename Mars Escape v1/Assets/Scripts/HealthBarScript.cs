using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour {

	public Vector3 newLocalScale;
	public Vector3 newLocalPosition;
	public GameObject parentObject;
	public string parentObjectTag;
	public Vector3 CharacterScale;

	// Use this for initialization
	void Start () {
		newLocalScale = transform.localScale;
		newLocalPosition = transform.localPosition;
		parentObject = this.transform.parent.gameObject;
		parentObjectTag = parentObject.tag;
	}
	
	// Update is called once per frame
	void Update () {;
		if(parentObjectTag == "Cosmonaut")
			newLocalScale.x = (float)(0.01 * parentObject.GetComponent<cosmonautBase>().currentHP);
		else
			if(parentObjectTag == "Maraptor")
				newLocalScale.x = (float)(0.005 * parentObject.GetComponent<maraptorBase>().currentHP);

		// Keep the healthbar localscales cosntant to the global scales
		CharacterScale = parentObject.transform.localScale;

		if ((CharacterScale.x < 0 && newLocalScale.x > 0) || (CharacterScale.x > 0 && newLocalScale.x < 0))
			newLocalScale.x *= -1;
		
		if((newLocalScale.x < 0 && newLocalPosition.x < 0) || (newLocalScale.x > 0 && newLocalPosition.x > 0))
			newLocalPosition.x *= -1;
		
		transform.localScale = newLocalScale;
		transform.localPosition = newLocalPosition;
	}
}