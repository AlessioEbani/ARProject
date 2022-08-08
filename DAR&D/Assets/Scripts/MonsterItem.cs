using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterItem : MonoBehaviour {
    public TextMeshProUGUI name;
    public Image image;
    public TextMeshProUGUI description;

    public Pawn pawn;

    public float cr;

    public void Set(Pawn pawn) {
        this.pawn = pawn;
        name.text = pawn.name;
        image.sprite = pawn.sprite;
        description.text = GetDescription(pawn.CR,pawn.alignment,pawn.description,pawn.type);
        cr = GetCR(pawn.CR);
    }

    private float GetCR(string cr) {
        if (cr.IsNullOrEmpty()) {
            return 0;
        }
        if (cr.Contains("/")) {
            var splitted = cr.Split("/");
            float a = float.Parse(splitted[0]);
            float b = float.Parse(splitted[1]);
            return a / b;
        }
        return float.Parse(cr);
    }

    public string GetDescription(string CR, string alignment,string description, string type) {
        string newDescription = description;
        newDescription += $"\n <color=red> CR:{CR}</color> <color=green> Alignment:{alignment}</color> <color=blue> Type:{type}</color>";
        return newDescription;
    }
}