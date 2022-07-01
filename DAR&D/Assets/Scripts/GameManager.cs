using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour {
    
    public const string bundleName = "pawndatabundle";
    
    public List<Pawn> pawns;

    private void Awake() {
        var localAssetBundle=AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
        if (localAssetBundle == null) {
            Debug.LogError("Failed");
            return;
        }
        pawns=new List<Pawn>(localAssetBundle.LoadAllAssets<Pawn>());
    }
}