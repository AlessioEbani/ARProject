using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteButton : MonoBehaviour {
	private Animator animator;

	private bool touched;
	public bool Touched => touched;

	private void Start() {
		animator = GetComponent<Animator>();
	}

	private void Update() {
		TouchInputs();
	}

	private void TouchInputs() {
		if (Input.touchCount > 0) {
			touched = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
		}
	}
	
	public void TouchedWithPawn() {
		animator.SetTrigger("Highlighted");
	}
	
	public void Normal() {
		animator.SetTrigger("Normal");
	}
}