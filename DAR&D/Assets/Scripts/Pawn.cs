
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Pawn")]
public class Pawn:ScriptableObject {
	public string name;
	public string description;
	public bool isCharacter = true;
	public bool hasDetails;
	public string alignment;
	public string CR;
	public string type;
	public GameObject model;
	public Sprite sprite;
}