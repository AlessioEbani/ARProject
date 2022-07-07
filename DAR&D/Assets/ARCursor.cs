using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursor : MonoBehaviour {

	public GameObject cursorChildObject;
	public GameObject objectToPlace;
	public ARRaycastManager raycastManager;

	public bool useCursor;
	private Camera mainCamera;

	private void Start() {
		mainCamera = Camera.main;
		cursorChildObject.SetActive(useCursor);
	}

	private void Update() {
		if (useCursor) {
			UpdateCursor();
		}

		GetInputs();
	}

	private void GetInputs() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			if (useCursor) {
				Instantiate(objectToPlace, transform.position, transform.rotation);
			}
			else {
				List<ARRaycastHit> hits = new List<ARRaycastHit>();
				raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes);
				if (hits.Count > 0) {
					Instantiate(objectToPlace, hits[0].pose.position, hits[0].pose.rotation);
				}
			}
		}
	}

	private void UpdateCursor() {
		Vector2 screenPosition = mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		List<ARRaycastHit> hits = new List<ARRaycastHit>();
		raycastManager.Raycast(screenPosition, hits, TrackableType.Planes);
		if (hits.Count > 0) {
			transform.position = hits[0].pose.position;
			transform.rotation = hits[0].pose.rotation;
		}
	}
}