using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 0.025f;

	public TMP_InputField xSizeText;
	public TMP_InputField zSizeText;
	public TMP_InputField cellSizeText;
	public GameObject evenObject;
	public GameObject oddObject;
	public GameObject customGridPrefab;

	private Vector3 gridPosition;
	private Quaternion gridRotation;

	public List<Grid> spawnedGrids;

	private ARCursor arCursor;

	public event Action GridDestroyed;
	public event Action GridSpawned;
	public event Action GridSizeUpdated;

	private void Start() {
		arCursor = FindObjectOfType<ARCursor>();
		xSizeText.text = gridSize.x.ToString();
		zSizeText.text = gridSize.z.ToString();
		cellSizeText.text = gridUnit.ToString();
		xSizeText.onEndEdit.AddListener(OnEndEditX);
		zSizeText.onEndEdit.AddListener(OnEndEditZ);
		cellSizeText.onEndEdit.AddListener(OnEndEditSize);
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

	private void OnEndEditSize(string text) {
		var newSize = float.Parse(text,CultureInfo.InvariantCulture);
		if (newSize != gridUnit) {
			gridUnit = newSize;
		}
		GridSizeUpdated?.Invoke();
	}

	public void SpawnGrid(GridPreset gridPreset) {
		gridPosition = arCursor.transform.position;
		gridRotation = arCursor.transform.rotation;
		var newGrid = Instantiate(gridPreset.model, gridPosition, gridRotation).GetComponent<Grid>();
		newGrid.transform.localScale = new Vector3(gridUnit, gridUnit, gridUnit);
		if (gridPreset.name.ToLower().Contains("custom")) {
			InitCustomGrid(newGrid.transform);
		}
		spawnedGrids.Add(newGrid);
		GridSpawned?.Invoke();
	}

	private void InitCustomGrid(Transform newGrid) {
		for (int i = -gridSize.x / 2; i < gridSize.x / 2; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = -gridSize.z / 2; k < gridSize.z / 2; k++) {
					Vector3 newPosition = new Vector3(i, gridPosition.y, k);
					//newPosition *= gridUnit;
					GameObject obj;
					if ((i + k) % 2 == 0) {
						obj = Instantiate(evenObject, newGrid.transform);
					}
					else {
						obj = Instantiate(oddObject, newGrid.transform);
					}
					obj.transform.localPosition = newPosition;
					//obj.transform.localScale *= gridUnit;
				}
			}
		}
	}

	private void DestroyGrid() {
		GridDestroyed?.Invoke();
	}

	public void SpawnPawn(Pawn pawn, Vector3 position) {
		var obj = Instantiate(pawn.model, transform.position, Quaternion.identity);
		var pawnBehaviour = obj.GetComponent<PawnBehaviour>();
		pawnBehaviour.GridManager = this;
		pawnBehaviour.gameObject.transform.localScale *= gridUnit;
		pawnBehaviour.transform.parent = transform;
		if (pawnBehaviour.IsColliding()) {
			Debug.Log("Invalid position");
		}
		else {
			//Aggiungere controllo  su quale grid aggiungere  pawn con raycast
			//pawnInstances.Add(pawnBehaviour);
		}
	}

	public void SpawnPawn(Pawn pawn) {
		var obj = Instantiate(pawn.model, transform.position, Quaternion.identity);
		var pawnBehaviour = obj.GetComponent<PawnBehaviour>();
		pawnBehaviour.GridManager = this;
		pawnBehaviour.gameObject.transform.localScale *= gridUnit;
		pawnBehaviour.transform.parent = transform;
		if (pawnBehaviour.IsColliding()) {
			Debug.Log("Invalid position");
		}
		else {
			//Aggiungere controllo  su quale grid aggiungere  pawn con raycast
			//pawnInstances.Add(pawnBehaviour);
		}
	}

}