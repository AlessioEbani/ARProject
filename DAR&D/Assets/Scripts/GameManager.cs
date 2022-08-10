using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour {

	public const string pawnBundleName = "pawndatabundle";
	public const string gridBundleName = "griddatabundle";
	public static readonly LayerMask pawnLayerMask = (1<<6);
	public static readonly LayerMask groundLayerMask = (1<<7);
	
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
		
		OnGridPositioning();
	}

	private void Start() {
		GridManager.GridDestroyed += OnGridDeleted;
		GridManager.GridSpawned += OnGridSpawned;
	}

	private void Update() {
		PawnMoveInputs();
		if (dragging) {
			DragInputs();
		}
	}

	private void OnGridSpawned() {
		OnPawnPositioning();
	}

	private void OnGridDeleted() {
		OnGridPositioning();
	}

	private void OnGridPositioning() {
		///arDefaultPlane.gameObject.SetActive(true);
	}

	private void OnPawnPositioning() {
		///arDefaultPlane.gameObject.SetActive(false);
	}

	private void GridPositionInputs() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//GridManager.SpawnCustomGrid(ARCursor.transform.position);
		}
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			if (ARCursor.useCursor) {
				//GridManager.SpawnCustomGrid(ARCursor.transform.position);
			}
			else {
				List<ARRaycastHit> hits = new List<ARRaycastHit>();
				raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes);
				if (hits.Count > 0) {
					//GridManager.SpawnCustomGrid(hits[0].pose.position);
				}
			}
		}
	}

	private void PawnMoveInputs() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(Ray, out hit,100000,pawnLayerMask)) {
				dragging = true;
				objectDragged = hit.collider.transform.parent.gameObject.GetComponent<PawnBehaviour>();
				startingPosition = objectDragged.transform.position;
			}
		}
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(Ray, out hit,100000,pawnLayerMask)) {
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
			if (Physics.Raycast(Ray, out hit,100000,groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x,objectDragged.transform.position.y,hit.point.z);
			}
		
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
			if (Physics.Raycast(Ray, out hit,100000,groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x,objectDragged.transform.position.y,hit.point.z);
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
}