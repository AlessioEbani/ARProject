using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 1;

	public TMP_InputField xSizeText;
	public TMP_InputField zSizeText;
	public TMP_InputField cellSizeText;
	public GameObject evenObject;
	public GameObject oddObject;

	private Vector3 gridPosition;

	private List<PawnBehaviour> pawnInstances = new();
	private List<GameObject> tiles = new();

	public event Action GridDestroyed;
	public event Action GridSpawned;

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
			DestroyTiles();
		}
	}

	private void OnEndEditZ(string text) {
		var newZ = int.Parse(text);
		if (newZ != gridSize.z) {
			gridSize.z = newZ;
			DestroyTiles();
		}
	}

	private void OnEndEditSize(string text) {
		var newSize = float.Parse(text);
		if (newSize != gridUnit) {
			gridUnit = newSize;
			DestroyTiles();
		}
	}
	
	public void SpawnGrid(Vector3Int size, float cellSize, Vector3 worldPos) {
		gridSize = size;
		gridUnit = cellSize;
		gridPosition = worldPos;
		InitGrid();
	}

	public void SpawnGrid(Vector3 position) {
		gridPosition = position;
		InitGrid();
	}

	private void InitGrid() {
		if (tiles.Count > 0) {
			DestroyTiles();
		}
		transform.position = gridPosition;
		for (int i = -gridSize.x / 2; i < gridSize.x / 2; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = -gridSize.z / 2; k < gridSize.z / 2; k++) {
					Vector3 newPosition = new Vector3(i, gridPosition.y, k);
					newPosition *= gridUnit;
					GameObject obj;
					if ((i + k) % 2 == 0) {
						obj = Instantiate(evenObject, transform);
					}
					else {
						obj = Instantiate(oddObject, transform);
					}
					obj.transform.localPosition = newPosition;

					obj.transform.localScale *= gridUnit;
					tiles.Add(obj);
				}
			}
		}
		GridSpawned?.Invoke();
	}

	private void DestroyTiles() {
		foreach (GameObject tile in tiles) {
			Destroy(tile);
		}
		tiles.Clear();
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
			pawnInstances.Add(pawnBehaviour);
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
			pawnInstances.Add(pawnBehaviour);
		}
	}
}