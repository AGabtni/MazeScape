using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public LayerMask ignoreMask;
	public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;


	private Vector3 targetDirection;
	private float distance;

	// Use this for initialization
	void Awake () {
		targetDirection = transform.localPosition.normalized;
		distance = transform.localPosition.magnitude;
		GetComponent<Camera>().cullingMask = ~(1 << LayerMask.NameToLayer("Map"));
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 desiredCameraPos = transform.parent.TransformPoint (targetDirection * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast (transform.parent.position, desiredCameraPos, out hit,ignoreMask )) {
			distance = Mathf.Clamp ((hit.distance * 0.87f), minDistance, maxDistance);
				
				} else {
					distance = maxDistance;
				}

				transform.localPosition = Vector3.Lerp (transform.localPosition, targetDirection * distance, Time.deltaTime * smooth);
	}
}
