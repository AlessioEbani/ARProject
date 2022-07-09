using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARCursor : MonoBehaviour {

	public GameObject cursorChildObject;
	public ARRaycastManager raycastManager;
	
	private GridManager gridManager;
	public GridManager GridManager {
		get {
			if (!gridManager) {
				gridManager = FindObjectOfType<GridManager>();
			}
			return gridManager;
		}
	}

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