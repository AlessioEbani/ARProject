using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSearchBar : MonoBehaviour
{
    private const string RegexMatch = "";

	public GridItem itemPrefab;
	public Transform content;
	public Button alphabeticalButton;
	public TMP_InputField inputField;

	private List<GridItem> items;
	private GameManager gameManager;
	private bool alphabeticalOrder;

	private void Start() {
		gameManager = FindObjectOfType<GameManager>();
		items = new List<GridItem>();
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
			
		}
	}

	private void InstantiateAllData() {
		ClearData();
		foreach (GridPreset gridPreset in gameManager.gridPresets) {
			var obj = Instantiate(itemPrefab, content);
			obj.Set(gridPreset);
			obj.GetComponent<Button>().onClick.AddListener(delegate { SpawnGrid(gridPreset); });
			items.Add(obj);
		}
		SortDataByName();
	}

	private void SpawnGrid(GridPreset gridPreset) {
		gameManager.GridManager.SpawnGrid(gridPreset);
	}


	private void ClearData() {
		foreach (GridItem gridItem in items) {
			Destroy(gridItem.gameObject);
		}
		items.Clear();
	}

	private void SortDataByName() {
		items = items.OrderBy(x => x.gridPreset.name).ToList();
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

		for (int i = 0; i < items.Count; i++) {
			if (items[i].gridPreset.name.ToLower().Contains(pawnName.ToLower())) 
			{
				items[i].gameObject.SetActive(true);
			}
			else {
				items[i].gameObject.SetActive(false);
			}
		}
	}
}
