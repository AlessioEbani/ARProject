using UnityEngine;
using UnityEngine.UI;

public class CursorHider : MonoBehaviour {
	private Button self;
	private ARCursor arCursor;

	private void Awake() {
		self = GetComponent<Button>();
		arCursor = FindObjectOfType<ARCursor>();
		self.onClick.AddListener(ToggleCursor);
	}

	private void ToggleCursor() {
		arCursor.gameObject.SetActive(!arCursor.gameObject.activeSelf);
	}

}