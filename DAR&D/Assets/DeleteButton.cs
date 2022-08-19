using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
	private Animator animator;
	private Button button;

	private bool touched;
	public bool Touched => touched;

	private void Start() {
		animator = GetComponent<Animator>();
		button = GetComponent<Button>();
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

	public void OnPointerEnter(PointerEventData eventData) {
		touched = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		touched = false;
	}
}