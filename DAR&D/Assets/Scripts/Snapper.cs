using System;
using UnityEngine;

[ExecuteInEditMode]
public class Snapper : MonoBehaviour {
    private Vector3Int size;

    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            UpdateSize();
        }
    }

    void Start() {
        UpdateSize();
    }

    void Update() {
        SnapToGrid();
    }

    private void UpdateSize() {
        var localScale = transform.localScale;
        size = new Vector3Int(Mathf.CeilToInt(localScale.x), Mathf.CeilToInt(localScale.y), Mathf.CeilToInt(localScale.z));
    }

    private void SnapToGrid() {
        var currentPosition = transform.position;
        var posX = (size.x % 2 == 0) ? currentPosition.x - 0.5f : currentPosition.x;
        posX = Mathf.RoundToInt(posX);
        if (size.x % 2 == 0) {
            posX += 0.5f;
        }
        
        float posY = Mathf.RoundToInt(currentPosition.y);
        
        float posZ = (size.z % 2 == 0) ? currentPosition.z - 0.5f : currentPosition.z;
        posZ = Mathf.RoundToInt(posZ);
        if (size.z % 2 == 0) {
            posZ += 0.5f;
        }
        var position = new Vector3(posX,posY,posZ);
        transform.position = position;
    }
}
