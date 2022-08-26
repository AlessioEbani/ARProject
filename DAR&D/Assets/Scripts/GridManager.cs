using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 0.025f;

	
	public GameObject evenObject;
	public GameObject oddObject;

	public List<CanvasOverlay> canvasOverlays;
	
	private Vector3 gridPosition;
	private Quaternion gridRotation;

	public List<Grid> spawnedGrids;

	private ARCursor arCursor;
	private Camera mainCamera;

	public event Action GridDestroyed;
	public event Action GridSpawned;
	public event Action GridSizeUpdated;

	private void Start() {
		arCursor = FindObjectOfType<ARCursor>();
		mainCamera = Camera.main;
		foreach (CanvasOverlay canvasOverlay in canvasOverlays) {
			canvasOverlay.Init(gridSize,gridUnit);
			canvasOverlay.xSizeText.onEndEdit.AddListener(OnEndEditX);
			canvasOverlay.zSizeText.onEndEdit.AddListener(OnEndEditZ);
			canvasOverlay.cellSize.onValueChanged.AddListener(OnEditSize);
			canvasOverlay.deleteButton.onClick.AddListener(DeleteGrid);
		}
		arCursor.transform.localScale = new Vector3(gridUnit,gridUnit,gridUnit);
	}

	private void OnEndEditX(string text) {
		var newX = int.Parse(text);
		if (newX != gridSize.x) {
			gridSize.x = newX;
		}
	}

	private void OnEndEditZ(string text) {
		var newZ = int.Parse(text);
		if (newZ != gridSize.z) {
			gridSize.z = newZ;
		}
	}

	private void OnEditSize(float value) {
		var newSize = value;
		if (newSize != gridUnit) {
			gridUnit = newSize;
		}
		arCursor.transform.localScale = new Vector3(gridUnit,gridUnit,gridUnit);
		GridSizeUpdated?.Invoke();
	}

	public void SpawnGrid(GridPreset gridPreset) {
		gridPosition = arCursor.transform.position;
		gridRotation = arCursor.transform.rotation;
		var newGrid = Instantiate(gridPreset.model, gridPosition, gridRotation).GetComponent<Grid>();
		newGrid.AddComponent<ARAnchor>();
		newGrid.transform.localScale = new Vector3(gridUnit, gridUnit, gridUnit);
		if (gridPreset.name.ToLower().Contains("custom")) {
			InitCustomGrid(newGrid.transform);
		}
		spawnedGrids.Add(newGrid);
		GridSpawned?.Invoke();
	}

	private void InitCustomGrid(Transform newGrid) {
		var upperXLimit = gridSize.x % 2 == 0 ? gridSize.x / 2 : (gridSize.x / 2) + 1;
		var upperZLimit = gridSize.z % 2 == 0 ? gridSize.z / 2 : (gridSize.z / 2) + 1;
		
		for (int i = -gridSize.x / 2; i < upperXLimit; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = -gridSize.z / 2; k < upperZLimit; k++) {
					Vector3 newPosition = new Vector3(i, gridPosition.y, k);
					GameObject obj;
					if ((i + k) % 2 == 0) {
						obj = Instantiate(evenObject, newGrid.transform);
					}
					else {
						obj = Instantiate(oddObject, newGrid.transform);
					}
					obj.transform.localPosition = newPosition;
				}
			}
		}
	}

	private void DeleteGrid() {
		RaycastHit hit;
		var screenPosition=mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		var Ray = mainCamera.ScreenPointToRay(screenPosition);
		if (Physics.Raycast(Ray, out hit, 100000, GameManager.groundLayerMask)) {
			var gridParent=hit.collider.GetComponentInParent<Grid>();
			spawnedGrids.Remove(gridParent);
			Destroy(gridParent.gameObject);
			GridDestroyed?.Invoke();
		}
	}

	public void SpawnPawn(Pawn pawn) {
		var screenPosition=mainCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		var Ray = mainCamera.ScreenPointToRay(screenPosition);
		if (Physics.Raycast(Ray, out RaycastHit hit,100000,GameManager.groundLayerMask)) {
			var gridParent=hit.collider.GetComponentInParent<Grid>();
			var pawnParent = gridParent.pawnParent ? gridParent.pawnParent : gridParent.transform;
			var obj = Instantiate(pawn.model, hit.point, Quaternion.identity,pawnParent);
			var pawnBehaviour = obj.GetComponent<PawnBehaviour>();
			pawnBehaviour.parentGrid = gridParent;
			if (pawnBehaviour.IsColliding()) {
				//find  nearest free space
				Debug.Log("Invalid position");
				Destroy(pawnBehaviour.gameObject);
			}
			else {
				gridParent.instantiedPawns.Add(pawnBehaviour);
			}
		}
		
	}

	public void DeletePawn(PawnBehaviour objectDragged) {
		if (objectDragged.parentGrid == null) {
			foreach (Grid spawnedGrid in spawnedGrids) {
				spawnedGrid.instantiedPawns.RemoveAll(item => item == null);
			}
		}
		else {
			objectDragged.parentGrid.instantiedPawns.Remove(objectDragged);
		}
		Destroy(objectDragged.gameObject);
	}
}