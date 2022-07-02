using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
	public Vector3Int gridSize;
	public float gridUnit = 1;

	private List<PawnBehaviour> pawnInstances=new();

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