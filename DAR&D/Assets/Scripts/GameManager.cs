using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
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
	public DeleteButton deleteButton1;
	public DeleteButton deleteButton2;
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
				objectDragged = hit.collider.GetComponent<PawnBehaviour>();
				if (objectDragged.movable) {
					startingPosition = objectDragged.transform.position;
					dragging = true;
				}
				else {
					dragging = false;
					objectDragged = null;
				}
			}
		}
	}

	private void DragInputs() {
		if (Input.touchCount > 0) {
			RaycastHit hit;
			var ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
			if (Physics.Raycast(ray, out hit, 100000, groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x, objectDragged.transform.position.y, hit.point.z);
			}

			if (DeleteButtonTouched()) {
				SetTouched();
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					GridManager.DeletePawn(objectDragged);
					SetNormal();
					objectDragged = null;
					dragging = false;
				}
			}
			else {
				SetNormal();
				if (Input.GetTouch(0).phase == TouchPhase.Ended) {
					dragging = false;
					if (objectDragged.IsColliding()) {
						objectDragged.transform.position = startingPosition;
					}
					objectDragged = null;
				}
			}
		}

		if (Input.GetMouseButton(0)) {
			RaycastHit hit;
			var Ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(Ray, out hit, 100000, groundLayerMask)) {
				objectDragged.transform.position = new Vector3(hit.point.x, objectDragged.transform.position.y, hit.point.z);
			}
		}
		
	}

	private bool DeleteButtonTouched() {
		return deleteButton1.Touched || deleteButton2.Touched;
	}

	private void SetNormal() {
		deleteButton1.Normal();
		deleteButton2.Normal();
	}

	private void SetTouched() {
		deleteButton1.TouchedWithPawn();
		deleteButton2.TouchedWithPawn();
	}
}