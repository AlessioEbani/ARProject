using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour {
	public TextMeshProUGUI name;
	public Image image;
	public TextMeshProUGUI description;

	public GridPreset gridPreset;

	public void Set(GridPreset gridPreset) {
		name.text = gridPreset.gridName;
		image.sprite = gridPreset.sprite;
		description.text = gridPreset.description;
		this.gridPreset = gridPreset;
	}
    
}