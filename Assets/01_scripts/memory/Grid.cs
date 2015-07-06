using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour {

	public Tile tilePrefab;
	public float scale = 1.5f;
	public int width = 7;
	public int height = 4;
	public int remainingPairs;

	public TileDataSet tileDataSet;
	public VideoPlayer videoPlayer;
	public HighscoreHandler highscoreHandler;

	Tile[,] tiles;
	List<TileData> tileData;
	// Use this for initialization
	void Start () {
		GenerateGrid(GameSettings.Instance.memoryWidth, GameSettings.Instance.memoryHeight);
		StartCoroutine (GameLogic());
	}

	void GenerateGrid(int width, int height) {
		// check if dividable by two
		if((width * height) % 2 == 1) {
			Debug.LogError (width + " * " + height + " is not possible, no multiple of 2");
			return;
		}
		var pairCount = width * height / 2;
		var centerX = (width - 1) / 2.0f;
		var centerY = (height - 1) / 2.0f;

		tileDataSet.PrepareShuffledTiles();

		// generate the memory tile data (half the amount of tiles)
		tileData = new List<TileData>();

		for(int i = 0; i < pairCount; i++) {
			var randomData = GenerateTileData();
			tileData.Add (randomData);
			tileData.Add (randomData);
		}

		tileData.Shuffle();

		tiles = new Tile[width,height];

		for(int x = 0; x < width; x++) {
			for(int y = 0; y < height; y++) {
				var t = Instantiate <Tile> (tilePrefab);
				t.AssignData(tileData[y * width + x]);
				t.transform.localPosition = new Vector2(x - centerX,y - centerY) * scale;
				t.name = "tile_" + x + "_" + y;
				t.parentGrid = this;
				t.pos = new Vector2(x,y);
				tiles[x,y] = t;
			}
		}

		remainingPairs = pairCount;
	}
	Tile clickedTile = null;
	Tile first, second;

	public bool autoMode = false;

	public static List<Tile> ConvertToList(Tile[,] array) {
		var r = new List<Tile>();

		var w = array.GetLength(0);
		var h = array.GetLength(1);
		for(int i = 0; i < w; i++)
			for(int j = 0; j < h; j++)
				r.Add (array[i,j]);

		return r;
	}

	public void HideAllTiles() {
		for(int i = 0; i < tiles.GetLength(0); i++)
			for(int j = 0; j < tiles.GetLength(1); j++)
				tiles[i,j].HideNow();
	}

	IEnumerator GameLogic()
	{
		var foundList = new List<TileData>();
		int usedTries = 0;
		var startTime = Time.time;

		if(autoMode)
		{
			yield return new WaitForSeconds(0.5f);

			var tmp = ConvertToList(tiles);

			while(remainingPairs > 0) {
				first = tmp.First (x => !x.solved);
				second = tmp.FindLast(x => TileData.Matches (first.data, x.data));

				first.Show ();
				yield return new WaitForSeconds(0.5f);
				second.Show ();
				yield return new WaitForSeconds(0.5f);

				first.Remove ();
				second.Remove ();

				foundList.Add (first.data);
				remainingPairs--;

				// hack to not destroy the highscore
				usedTries += 10;
				startTime -= 10;


			}
		}
		else
		while(remainingPairs > 0)
		{
			// wait for first tile
			while(clickedTile == null) yield return null;
			first = clickedTile; clickedTile = null;
			if(second != null) second.FadeOut();
			first.Show ();

			// wait for second
			while(clickedTile == null || clickedTile == first) yield return null;
			second = clickedTile; clickedTile = null;
			if(first != null) first.FadeOut();

			var tilesMatch = TileData.Matches(first.data, second.data);

			// play audio of second tile only if they don't match
			// - otherwise sound is played with the video
			second.Show (!tilesMatch);

			// compare their content
			if(tilesMatch) {
				foundList.Add (first.data);
				yield return StartCoroutine(FoundMatch(first, second));
				// add score
				remainingPairs--;
			}
			else {
				yield return StartCoroutine(NoMatch(first, second));
			}

			usedTries++;
		}

		var usedTime = Time.time - startTime;
		yield return new WaitForSeconds(0.5f);

		// playing single video list
//		yield return StartCoroutine(videoPlayer.PlayList(foundList.Select (x => x.movie).ToArray()));
		// playing composition
		yield return StartCoroutine(videoPlayer.Play(foundList.Select (x => x.movie).ToArray()));

		// game is over - do some cool stuff
		print ("game over! thanks for playing!");

		// show usedTime and usedTries
		print ("used time: " + usedTime + ", " + "usedTries: " + usedTries);

		highscoreHandler.ShowResult(usedTime, usedTries);

//		Application.LoadLevel ("menu");
	}

	IEnumerator FoundMatch(Tile a, Tile b) {
		yield return new WaitForSeconds(0.5f);

		// remove them,
		a.Remove ();
		b.Remove ();

		// play video
		yield return StartCoroutine(videoPlayer.Play(a.data.movie));
	}

	IEnumerator NoMatch(Tile a, Tile b) {
		yield return new WaitForSeconds(1.0f);

		// hide both again
		a.Hide ();
		b.Hide ();
	}

	public void OnClickTile(Tile t) {
		clickedTile = t;
	}

	TileData GenerateTileData() {
		// dont generate random data, but get something from the TileDataSet

		return tileDataSet.Next();

//		var td = new TileData();
//		td.color = new HSV(Random.value, 0.6f, 0.9f);
//		return td;
	}
}
