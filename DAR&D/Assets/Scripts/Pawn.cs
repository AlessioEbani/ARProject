
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pawn")]
public class Pawn:ScriptableObject {
	public string name;
	public string alignment;
	public string CR;
	public string type;
	public string description;
	public GameObject model;
	public Sprite sprite;
}