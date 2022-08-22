using UnityEngine;
using UnityEngine.UI;

public class UIHider : MonoBehaviour {
    private Button self;
    public UIManager uiManager;
    
    private void Awake() {
        self = GetComponent<Button>();
        self.onClick.AddListener(ToggleUI);
    }

    private void ToggleUI() {
        uiManager.ToggleUI();
    }
}
