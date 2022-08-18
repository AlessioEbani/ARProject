using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour {

	private const string pawnBundleName = "pawndatabundle";
	private const string gridBundleName = "griddatabundle";
	private static readonly LayerMask pawnLayerMask = (1 << 6);

	public static readonly LayerMask groundLayerMask = (1 << 7);

	public List<Pawn> pawns;
	public List<GridPreset> gridPresets;

	private GridManager gridManager;
	public GridManager GridManager {
		get {
			if (!gridManager) {
				gridManager = FindObjectOfType<GridManager>();
			}
			return gridManager;
		}
	}

	public ARCursor arCursor;
	public ARCursor ARCursor {
		get => arCursor;
		set => arCursor = value;
	}

	private Camera mainCamera;
	private bool dragging;
	private PawnBehaviour objectDragged;
	private Vector3 startingPosition;

	public ARRaycastManager raycastManager;
	public DeleteButton deleteButton;
	public TextMeshProUGUI phaseText;

	private void Awake() {
		mainCamera = Camera.main;
		var localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, pawnBundleName));
		if (localAssetBundle == null) {
			Debug.LogError("Failed");
			return;
		}
		pawns = new List<Pawn>(localAssetBundle.LoadAllAssets<Pawn>());
		localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, gridBundleName));
		if (localAssetBundle == null) {
			Debug.LogError("Failed");
			return;
		}
		gridPresets = new List<GridPreset>(localAssetBundle.LoadAllAssets<GridPreset>());
	}

	private void Update() {
		PawnMoveInputs();
		if (dragging) {
			DragInputs();
		}
	}

	private void PawnMoveInputs() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(Ray, out hit, 100000, pawnLayerMask)) {
				dragging = true;
				objectDragged = hit.collider.transform.parent.gameObject.GetComponent<PawnBehaviour>();
				startingPosition = objectDragged.transform.position;
			}
		}
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(Ray, out hit, 100000, pawnLayerMask)) {
				dragging = true;
				objectDragged = hit.collider.GetComponentInParent<PawnBehaviour>();
				startingPosition = objectDragged.transform.position;
			}
		}
	}

	private void DragInputs() {
		if (Input.touchCount > 0) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(Ray, out hit, 100000, groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x, objectDragged.transform.position.y, hit.point.z);
			}

			deleteButton.gameObject.transform.po
			if (Input.GetTouch(0).position.)

				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					dragging = false;
					if (objectDragged.IsColliding()) {
						objectDragged.transform.position = startingPosition;
					}
					objectDragged = null;
				}
		}

		if (Input.GetMouseButton(0)) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(Ray, out hit, 100000, groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x, objectDragged.transform.position.y, hit.point.z);
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			dragging = false;
			if (objectDragged.IsColliding()) {
				objectDragged.transform.position = startingPosition;
			}
			objectDragged = null;
		}
	}

	private bool checkOverlap(Vector2 positionToOverlap,Vector3 scale, Vector2 currentPosition) {
		bool overlap = true;
		if (currentPosition.x>positionToOverlap.x+scale.x ||
		    currentPosition.x<positionToOverlap.x-scale.x ||
		    currentPosition.y<positionToOverlap.y-scale.y ||
		    currentPosition.y<positionToOverlap.y+scale.y) {
			overlap = false;
		}
		return overlap;
	}
}