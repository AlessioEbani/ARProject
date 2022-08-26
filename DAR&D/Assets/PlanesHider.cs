using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlanesHider : MonoBehaviour {
    public ARPlaneMeshVisualizer planeMeshVisualizer;
    public LineRenderer lineRenderer;
    public MeshRenderer meshRenderer;
    private Button button;

    private bool visible = true;
    public bool Visible {
        get => visible;
        set {
            visible = value;
            lineRenderer.enabled = visible;
            planeMeshVisualizer.enabled = visible;
            meshRenderer.enabled = visible;
        }
    }

    private void Awake() {
        button = GetComponent<Button>();
        button.onClick.AddListener(TogglePlanes);
        Visible = true;
    }


    private void TogglePlanes() {
        Visible = !Visible;
    }
}
