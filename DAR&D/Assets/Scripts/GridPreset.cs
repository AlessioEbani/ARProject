using UnityEngine;

[CreateAssetMenu(menuName = "Data/Grid")]
public class GridPreset:ScriptableObject {
	public string name;
	public string description;
	public GameObject model;
	public Sprite sprite;
}