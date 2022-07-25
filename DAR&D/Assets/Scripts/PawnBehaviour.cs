using System;
using UnityEngine;

public class PawnBehaviour : MonoBehaviour {
	public Pawn pawn;
	private int collision = 0;

	private GridManager gridManager;
	public GridManager GridManager {
		get {
			if (!gridManager) {
				gridManager = FindObjectOfType<GridManager>();
			}
			return gridManager;
		}
		set { gridManager = value; }
	}

	private Vector3Int size;

	private void Start() {
		collision = 0;
		UpdateSize();
		SnapToGrid();
	}

	private void Update() {
		SnapToGrid();
	}

	public bool IsColliding() {
		SnapToGrid();
		if (collision > 0) {
			return true;
		}
		return false;
	}

	private void UpdateSize() {
		var localScale = transform.localScale;
		size = new Vector3Int(Mathf.CeilToInt(localScale.x), Mathf.CeilToInt(localScale.y), Mathf.CeilToInt(localScale.z));
	}

	private void SnapToGrid() {
		var currentPosition = transform.position;
		var value = currentPosition.x / GridManager.gridUnit;
		var posX = (size.x % 2 == 0) ? value - GridManager.gridUnit / 2 : value;
		posX = Mathf.RoundToInt(posX);
		posX *= GridManager.gridUnit;
		if (size.x % 2 == 0) {
			posX += GridManager.gridUnit / 2;
		}

		value = currentPosition.y / GridManager.gridUnit;
		var posY = (size.y % 2 == 0) ? value - GridManager.gridUnit / 2 : value;
		posY = Mathf.RoundToInt(posY);
		posY *= GridManager.gridUnit;
		if (size.y % 2 == 0) {
			posY += GridManager.gridUnit / 2;
		}

		value = currentPosition.z / GridManager.gridUnit;
		float posZ = (size.z % 2 == 0) ? value - GridManager.gridUnit / 2 : value;
		posZ = Mathf.RoundToInt(posZ);
		posZ *= GridManager.gridUnit;
		if (size.z % 2 == 0) {
			posZ += GridManager.gridUnit / 2;
		}
		var position = new Vector3(posX, posY, posZ);
		transform.position = position;
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Pawn")) {
			collision++;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Pawn")) {
			collision--;
		}
	}
}