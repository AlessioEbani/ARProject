using UnityEngine;

public class UIManager : MonoBehaviour {
	public GameObject portraitCanvas;
	public GameObject landscapeCanvas;

	public bool isPortrait;

	private void Update() {
		if (!isPortrait) {
			if (Input.deviceOrientation == DeviceOrientation.Portrait ||
			    Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				isPortrait = true;
				landscapeCanvas.SetActive(false);
				portraitCanvas.SetActive(true);
			}
		}
		else {
			if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft ||
			    Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
				isPortrait = false;
				portraitCanvas.SetActive(false);
				landscapeCanvas.SetActive(true);
			}
		}
	}

}