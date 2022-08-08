using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 1;

	public TMP_InputField xSizeText;
	public TMP_InputField zSizeText;
	public TMP_InputField cellSizeText;
	public GameObject evenObject;
	public GameObject oddObject;
	public GameObject customGridPrefab;
	
	private Vector3 gridPosition;
	private Quaternion gridRotation;
	
	public List<Grid> spawnedGrids;

	public event Action GridDestroyed;
	public event Action GridSpawned;
	public event Action GridSizeUpdated;

	private void Start() {
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
		var newSize = float.Parse(text);
		if (newSize != gridUnit) {
			gridUnit = newSize;
		}
		GridSizeUpdated?.Invoke();
	}
	
	public void SpawnCustomGrid(Vector3Int size, float cellSize, Vector3 worldPos) {
		gridSize = size;
		gridUnit = cellSize;
		gridPosition = worldPos;
		InitCustomGrid();
	}

	public void SpawnCustomGrid(Vector3 position) {
		gridPosition = position;
		InitCustomGrid();
	}
	
	public void SpawnGrid(Vector3 position,GridPreset gridPreset) {
		gridPosition = position;
		var newGrid = Instantiate(gridPreset.model).GetComponent<Grid>();
		spawnedGrids.Add(newGrid);
		GridSpawned?.Invoke();
	}

	private void InitCustomGrid() {
		var newGrid = Instantiate(customGridPrefab).GetComponent<Grid>();
		newGrid.transform.position = gridPosition;
		newGrid.transform.rotation = gridRotation;
		for (int i = -gridSize.x / 2; i < gridSize.x / 2; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = -gridSize.z / 2; k < gridSize.z / 2; k++) {
					Vector3 newPosition = new Vector3(i, gridPosition.y, k);
					newPosition *= gridUnit;
					GameObject obj;
					if ((i + k) % 2 == 0) {
						obj = Instantiate(evenObject, newGrid.transform);
					}
					else {
						obj = Instantiate(oddObject, newGrid.transform);
					}
					obj.transform.localPosition = newPosition;

					obj.transform.localScale *= gridUnit;
				}
			}
		}
		spawnedGrids.Add(newGrid);
		GridSpawned?.Invoke();
	}

	private void DestroyGrid() {
		GridDestroyed?.Invoke();
	}

	public void SpawnPawn( Pawn pawn,Vector3 position) {
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