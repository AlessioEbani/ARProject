using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Phase{GridPositioning,PawnPositioning}

public class GameManager : MonoBehaviour {
    
    public const string bundleName = "pawndatabundle";
    
    public List<Pawn> pawns;

    private GridManager gridManager;
    public GridManager GridManager {
        get {
            if (!gridManager) {
                gridManager = FindObjectOfType<GridManager>();
            }
            return gridManager;
        }
    }

    private Phase currentPhase;
    public Phase CurrentPhase => currentPhase;

    private void Awake() {
        var localAssetBundle=AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        if (localAssetBundle == null) {
            Debug.LogError("Failed");
            return;
        }
        pawns=new List<Pawn>(localAssetBundle.LoadAllAssets<Pawn>());
        currentPhase = Phase.GridPositioning;
    }
    
    private void Start() {
        //Debugging spawner
		GridManager.Spawn(new Vector3Int(0,0,0),pawns[0]);
    }

    private void Update() {
        
    }

    private void SpawnGrid() {
        currentPhase = Phase.PawnPositioning;
    }

    private void DeleteGrid() {
        currentPhase = Phase.GridPositioning;
    }
    
}