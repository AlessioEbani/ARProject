using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIPanel {
	public Button button;
	public GameObject panel;
}

public class SearchBarToggle : MonoBehaviour {
	[SerializeField] private List<UIPanel> uiPanels;

	private void Awake() {
		for (int i = 0; i < uiPanels.Count; i++) {
			int index = i;
			uiPanels[i].button.onClick.AddListener(delegate { ActivatePanel(index); });
		}
		ActivatePanel(0);
	}

	private void ActivatePanel(int index) {
		for (int i = 0; i < uiPanels.Count; i++) {
			uiPanels[i].panel.SetActive(i == index);
		}
	}

}