using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
	private Animator animator;

	private bool touched;
	public bool Touched => touched;

	private void Start() {
		animator = GetComponent<Animator>();
	}

	public void TouchedWithPawn() {
		animator.SetTrigger("Highlighted");
	}
	
	public void Normal() {
		animator.SetTrigger("Normal");
	}

	public void OnPointerEnter(PointerEventData eventData) {
		touched = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		touched = false;
	}
}