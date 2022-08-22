using UnityEngine;

public class UIManager : MonoBehaviour {
	public GameObject portraitCanvas;
	public GameObject landscapeCanvas;

	public bool isPortrait;
	public bool IsPortrait {
		get => isPortrait;
		set {
			isPortrait = value;
			landscapeCanvas.SetActive(!isPortrait && isEnabled);
			portraitCanvas.SetActive(isPortrait && isEnabled);
		}
	}

	private bool isEnabled = true;
	public bool IsEnabled {
		get => isEnabled;
		set {
			isEnabled = value;
			landscapeCanvas.SetActive(!isPortrait && isEnabled);
			portraitCanvas.SetActive(isPortrait && isEnabled);
		}
	}

	private void Update() {
		if (!isPortrait) {
			if (Input.deviceOrientation == DeviceOrientation.Portrait ||
			    Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				IsPortrait = true;
			}
		}
		else {
			if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
			    Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
				IsPortrait = false;
			}
		}
	}

	public void ToggleUI() {
		IsEnabled = !IsEnabled;
	}

}