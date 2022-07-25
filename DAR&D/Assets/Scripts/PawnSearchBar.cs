using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawnSearchBar : MonoBehaviour {
	private const string RegexMatch = "^(?'Name'([a-zA-Z]+))? ?(CR((?'CR'(\\d+)|(1\\/\\d))))? ?(\\/(?'Alignment'((L|N|C)?)((B|N|B)?)))?$";

	public MonsterItem itemPrefab;
	public Transform content;
	public Button alphabeticalButton;
	public TMP_InputField inputField;

	private List<MonsterItem> items;
	private GameManager gameManager;
	private bool alphabeticalOrder;

	private void Start() {
		gameManager = FindObjectOfType<GameManager>();
		items = new List<MonsterItem>();
		alphabeticalOrder = true;
		alphabeticalButton.onClick.AddListener(ToggleSort);
		inputField.onEndEdit.AddListener(FilterSearch);
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
		if (gameManager.CurrentPhase == Phase.PawnPositioning) {
			gameManager.GridManager.SpawnPawn(pawn);
		}
	}

	private void ClearData() {
		foreach (MonsterItem monsterItem in items) {
			Destroy(monsterItem.gameObject);
		}
		items.Clear();
	}

	private void SortDataByName() {
		items = items.OrderBy(x => x.pawn.name).ThenBy(x => x.cr).ToList();
		ReorderHierarchy();
	}

	private void SortDataByCR() {
		items = items.OrderBy(x => x.cr).ThenBy(x => x.pawn.name).ToList();
		ReorderHierarchy();
	}

	private void ReorderHierarchy() {
		for (int i = 0; i < items.Count; i++) {
			items[i].transform.SetSiblingIndex(i);
		}
	}

	private void FilterSearch(string input) {
		var match = Regex.Match(input, RegexMatch);
		string pawnName = !match.Groups["Name"].Value.IsNullOrEmpty() ? match.Groups["Name"].Value : "";
		string cr = !match.Groups["CR"].Value.IsNullOrEmpty() ? match.Groups["CR"].Value : "";
		string alignment = !match.Groups["Alignment"].Value.IsNullOrEmpty() ? match.Groups["Alignment"].Value : "";

		for (int i = 0; i < items.Count; i++) {
			if (items[i].pawn.name.ToLower().Contains(pawnName.ToLower()) &&
			    items[i].pawn.CR.ToLower().Contains(cr.ToLower()) &&
			    items[i].pawn.alignment.ToLower().Contains(alignment.ToLower())) 
			{
				items[i].gameObject.SetActive(true);
			}
			else {
				items[i].gameObject.SetActive(false);
			}
		}
	}
}