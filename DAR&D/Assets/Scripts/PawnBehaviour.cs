using UnityEngine;

[ExecuteInEditMode]
public class PawnBehaviour : Snapper {
	[HideInInspector] public Grid parentGrid;
	public Pawn pawn;
	public bool movable = true;
	private int collision = 0;

	protected override void Start() {
		collision = 0;
		SnapToGrid();
	}

	protected override void Update() {
		SnapToGrid();
	}

	public bool IsColliding() {
		SnapToGrid();
		if (collision > 0) {
			return true;
		}
		return false;
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