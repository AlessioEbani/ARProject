using System;
using UnityEngine;

[ExecuteInEditMode]
public class Snapper : MonoBehaviour {
    private Vector3Int size;
    private GridManager gridManager;
    
    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            UpdateSize();
        }
    }

    void Start() {
        UpdateSize();
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update() {
        if (gridManager) {
            SnapToGrid();
        }
    }

    private void UpdateSize() {
        var localScale = transform.localScale;
        size = new Vector3Int(Mathf.CeilToInt(localScale.x), Mathf.CeilToInt(localScale.y), Mathf.CeilToInt(localScale.z));
    }

    private void SnapToGrid() {
        var currentPosition = transform.position;
        var value = currentPosition.x / gridManager.gridUnit;
        var posX = (size.x % 2 == 0) ? value - gridManager.gridUnit/2 : value;
        posX = Mathf.RoundToInt(posX);
        posX *= gridManager.gridUnit;
        if (size.x % 2 == 0) {
            posX += gridManager.gridUnit/2;
        }
            
        value = currentPosition.y / gridManager.gridUnit;
        var posY = (size.y % 2 == 0) ? value - gridManager.gridUnit/2 : value;
        posY = Mathf.RoundToInt(currentPosition.y);
        posY*= gridManager.gridUnit;
        if (size.y % 2 == 0) {
            posY += gridManager.gridUnit/2;
        }
        
        value = currentPosition.z / gridManager.gridUnit;
        float posZ = (size.z % 2 == 0) ? value - gridManager.gridUnit/2 : value;
        posZ = Mathf.RoundToInt(posZ);
        posZ*= gridManager.gridUnit;
        if (size.z % 2 == 0) {
            posZ += gridManager.gridUnit/2;
        }
        var position = new Vector3(posX,posY,posZ);
        transform.position = position;
    }
}
