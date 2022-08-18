using System;
using UnityEngine;

public class LandscapeSwitch : MonoBehaviour {
	public GameObject portraitCanvas;
	public GameObject landscapeCanvas;

	public bool defaultPortraitCavas;

	private void Update() {
		if (Input.deviceOrientation == DeviceOrientation.Portrait ||
		    Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown ||
		    Input.deviceOrientation == DeviceOrientation.Unknown ||
		    Input.deviceOrientation == DeviceOrientation.FaceUp ||
		    Input.deviceOrientation == DeviceOrientation.FaceDown) { }
	}
}