using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI displayText;
 
    public void Update () {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        displayText.text = current.ToString();
    }
}
