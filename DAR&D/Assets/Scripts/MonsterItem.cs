using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterItem : MonoBehaviour {
    public TextMeshProUGUI name;
    public Image image;
    public TextMeshProUGUI description;

    public void Set(string name, Sprite sprite, string description) {
        this.name.text = name;
        image.sprite = sprite;
        this.description.text = description;
    }
}
