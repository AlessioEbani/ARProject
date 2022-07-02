using UnityEngine;

public class PawnBehaviour: MonoBehaviour {
	public Pawn pawn;
	public int collision=0;
	
	private Vector3Int size;

	private void Start() {
		collision = 0;
		UpdateSize();
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
		var posX = (size.x % 2 == 0) ? currentPosition.x - 0.5f : currentPosition.x;
		posX = Mathf.RoundToInt(posX);
		if (size.x % 2 == 0) {
			posX += 0.5f;
		}
		var posY = (size.y % 2 == 0) ? currentPosition.y - 0.5f : currentPosition.y;
		posY = Mathf.RoundToInt(currentPosition.y);
		if (size.y % 2 == 0) {
			posY += 0.5f;
		}
        
		float posZ = (size.z % 2 == 0) ? currentPosition.z - 0.5f : currentPosition.z;
		posZ = Mathf.RoundToInt(posZ);
		if (size.z % 2 == 0) {
			posZ += 0.5f;
		}
		var position = new Vector3(posX,posY,posZ);
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