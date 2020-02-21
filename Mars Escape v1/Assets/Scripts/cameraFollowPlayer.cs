using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour {

	public Transform target; //the object the carema will be following
	public float smoothing; //dampening effect (how quickly the camera follows or unfollows)

	Vector3 modifiedTarget;
	Vector3 offset;
	float lowY;

	// Use this for initialization
	void Start () {
		this.transform.position = modifiedTarget = new Vector3 (target.position.x, target.position.y, this.transform.position.z);
		offset = new Vector3 (0, 1, 0);
		lowY = -34;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
			modifiedTarget = new Vector3 (target.position.x, target.position.y, this.transform.position.z);
			Vector3 targetCamPos = modifiedTarget + offset;

			this.transform.position = Vector3.Lerp (this.transform.position, targetCamPos, smoothing * Time.deltaTime);

			if (this.transform.position.y < lowY)
				this.transform.position = new Vector3 (this.transform.position.x, lowY, this.transform.position.z);
		}
	}
}
