using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Data/Grid")]
public class GridPreset:ScriptableObject {
	[FormerlySerializedAs("name")] public string gridName;
	public string description;
	public GameObject model;
	public Sprite sprite;
}