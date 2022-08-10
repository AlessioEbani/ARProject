using System;
using UnityEngine;

[ExecuteInEditMode]
public class Snapper : MonoBehaviour {
    private Vector3Int size;
    private GridManager gridManager;
    public GridManager GridManager {
        get {
            if (!gridManager) {
                gridManager = FindObjectOfType<GridManager>();
            }
            return gridManager;
        }
        set { gridManager = value; }
    }
    public bool useGridScale=false;
    public bool groundPivot;
    private float gridSize = 1;

    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            UpdateSize();
        }
    }

    protected virtual void Start() {
        UpdateSize();
        GridManager.GridSizeUpdated += UpdateSize;
        gridSize = useGridScale ? GridManager.gridUnit : 1;
    }

    protected virtual void Update() {
        SnapToGrid();
    }

    protected void UpdateSize() {
        var localScale = transform.localScale;
        size = new Vector3Int(Mathf.CeilToInt(localScale.x), Mathf.CeilToInt(localScale.y), Mathf.CeilToInt(localScale.z));
        
    }

    protected void SnapToGrid() {
        var currentPosition = transform.localPosition;
        var value = currentPosition.x / gridSize;
        var posX = (size.x % 2 == 0) ? value - gridSize/2 : value;
        posX = Mathf.RoundToInt(posX);
        posX *= gridSize;
        if (size.x % 2 == 0) {
            posX += gridSize/2;
        }
            
        value = currentPosition.y / gridSize;
        float posY;
        if (groundPivot || size.y % 2 != 0) {
            posY = value;
        }else {
            posY = value - gridSize / 2;
        }
        posY = Mathf.RoundToInt(posY);
        posY*= gridSize;
        if (!groundPivot && size.y % 2 == 0) {
            posY += gridSize/2;
        }
        
        value = currentPosition.z / gridSize;
        float posZ = (size.z % 2 == 0) ? value - gridSize/2 : value;
        posZ = Mathf.RoundToInt(posZ);
        posZ*= gridSize;
        if (size.z % 2 == 0) {
            posZ += gridSize/2;
        }
        var position = new Vector3(posX,posY,posZ);
        transform.localPosition = position;
    }
}
