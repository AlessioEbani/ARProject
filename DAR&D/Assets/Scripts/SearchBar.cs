

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SearchBar : MonoBehaviour {
    public MonsterItem itemPrefab;
    public Transform content;
    public Button alphabeticalButton;

    private List<MonsterItem> items;
    private GameManager gameManager;
    private bool alphabeticalOrder;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        items = new List<MonsterItem>();
        alphabeticalOrder = true;
        alphabeticalButton.onClick.AddListener(ToggleSort);
        InstantiateAllData();
    }

    private void ToggleSort() {
        alphabeticalOrder = !alphabeticalOrder;
        if (alphabeticalOrder) {
            SortDataByName();
        }
        else {
            SortDataByCR();
        }
    }

    private void InstantiateAllData() {
        ClearData();
        foreach (Pawn pawn in gameManager.pawns) {
            var obj = Instantiate(itemPrefab, content);
            obj.Set(pawn);
            obj.GetComponent<Button>().onClick.AddListener(delegate { SpawnPawn(pawn); });
            items.Add(obj);
        }
        SortDataByName();
    }

    private void SpawnPawn(Pawn pawn) {
        gameManager.GridManager.SpawnPawn(pawn);
    }

    private void ClearData() {
        foreach (MonsterItem monsterItem in items) {
            Destroy(monsterItem.gameObject);
        }
        items.Clear();
    }

    private void SortDataByName() {
        items=items.OrderBy(x=>x.pawn.name).ThenBy(x=>x.cr).ToList();
        ReorderHierarchy();
    }

    private void SortDataByCR() {
        items=items.OrderBy(x=>x.cr).ThenBy(x=>x.pawn.name).ToList();
        ReorderHierarchy();
    }

    private void ReorderHierarchy() {
        for (int i = 0; i < items.Count; i++) {
            items[i].transform.SetSiblingIndex(i);
        }
    }
}
