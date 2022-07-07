using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 1;

	public GameObject evenObject;
	public GameObject oddObject;
	
	
	private List<PawnBehaviour> pawnInstances=new();

	private void Start() {
		InitGrid();
	}

	private void InitGrid() {
		for (int i = -gridSize.x / 2; i < gridSize.x/2; i++) {
			for (int j = 0; j < 1; j++) {
				for (int k = -gridSize.z/2; k < gridSize.z/2; k++) {
					Vector3 newPosition = new Vector3(i, j, k);
					newPosition *= gridUnit;
					GameObject obj;
					if ((i + k)  % 2 == 0) {
						obj=Instantiate(evenObject, transform);
					}
					else {
						obj=Instantiate(oddObject, transform);
					}
					obj.transform.localPosition = newPosition;

					obj.transform.localScale *= gridUnit;
				}
			}
		}
	}

	public void Spawn(Vector3 position, Pawn pawn) {
		var obj = Instantiate(pawn.model, position, Quaternion.identity, transform).GetComponent<PawnBehaviour>();
		if (obj.IsColliding()) {
			Debug.Log("Invalid position");
		}
		else {
			pawnInstances.Add(obj);
		}
	}
}