using UnityEngine;
using UnityEngine.UI;

public class CursorHider : MonoBehaviour {
	private Button self;
	private GameObject arCursor;

	private void Awake() {
		self = GetComponent<Button>();
		arCursor = FindObjectOfType<ARCursor>().gameObject;
		self.onClick.AddListener(ToggleCursor);
	}

	private void ToggleCursor() {
		arCursor.SetActive(!arCursor.activeSelf);
	}

}