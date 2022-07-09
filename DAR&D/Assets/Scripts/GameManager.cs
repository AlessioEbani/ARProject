using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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
    public Phase CurrentPhase {
        get { return currentPhase;}
        set {
            currentPhase = value;
            phaseText.text = Enum.GetName(typeof(Phase), currentPhase);
        }
    }
    
    public ARCursor arCursor;
    public ARCursor ARCursor {
        get => arCursor;
        set => arCursor = value;
    }

    public ARRaycastManager raycastManager;
    public TextMeshProUGUI phaseText;
    
    private void Awake() {
        var localAssetBundle=AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        if (localAssetBundle == null) {
            Debug.LogError("Failed");
            return;
        }
        pawns=new List<Pawn>(localAssetBundle.LoadAllAssets<Pawn>());
        CurrentPhase = Phase.GridPositioning;
    }
    
    private void Start() {
        GridManager.GridDestroyed += OnGridDeleted;
        GridManager.GridSpawned += OnGridSpawned;
    }

    private void Update() {
        switch (CurrentPhase) {
            case Phase.GridPositioning:
                GetGridPositionInputs();
                break;
            case Phase.PawnPositioning:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnGridSpawned() {
        CurrentPhase = Phase.PawnPositioning;
    }

    private void OnGridDeleted() {
        CurrentPhase = Phase.GridPositioning;
    }
    
    
    private void GetGridPositionInputs() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GridManager.SpawnGrid(ARCursor.transform.position);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            if (ARCursor.useCursor) {
                GridManager.SpawnGrid(ARCursor.transform.position);
            }
            else {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.Planes);
                if (hits.Count > 0) {
                    GridManager.SpawnGrid(hits[0].pose.position);
					
                }
            }
            
        }
    }
}