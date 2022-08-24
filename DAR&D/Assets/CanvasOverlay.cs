using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasOverlay : MonoBehaviour
{
	public TMP_InputField xSizeText;
	public TMP_InputField zSizeText;
	public Slider cellSize;
	public Button deleteButton;

	public void Init(Vector3 gridSize,float  gridUnit) {
		xSizeText.text = gridSize.x.ToString();
		zSizeText.text = gridSize.z.ToString();
		cellSize.value = gridUnit;
	}

	public void BindActions(UnityAction<string> xsize,UnityAction<string>  zsize,UnityAction<float> cellSizeAction,UnityAction deleteButtonAction) {
		xSizeText.onEndEdit.AddListener(xsize);
		zSizeText.onEndEdit.AddListener(zsize);
		cellSize.onValueChanged.AddListener(cellSizeAction);
		deleteButton.onClick.AddListener(deleteButtonAction);
		
	}
}
