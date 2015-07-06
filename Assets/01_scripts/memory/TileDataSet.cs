using UnityEngine;
using System.Collections;

public class TileDataSet : MonoBehaviour {

	[HideInInspector]
	public class Category {
		public string name = "category";
		public TileData[] tiles;
	}

	public Category[] categories;
	public int selectedCategory = 0;
	int currentTile = 0;

	void Start() {
		// build category data from children
		categories = new Category[transform.childCount];
		for(int i = 0; i < categories.Length; i++) {
			categories[i] = new Category();
			categories[i].name = transform.GetChild (i).name;
			categories[i].tiles = transform.GetChild(i).GetComponentsInChildren<TileData>();
		}
	}

	TileData[] tds;
	public void PrepareShuffledTiles() {
		// take the tiles from the current category
		// create a temporary list for it
		tds = categories[selectedCategory].tiles;
		// shuffle it
		tds.Shuffle();
		currentTile = 0;
	}

	public TileData Next() {
		currentTile %= tds.Length;
		return tds[currentTile++];
	}

	public TileData[] GetCurrentDataset() {
		return tds;
	}
}
