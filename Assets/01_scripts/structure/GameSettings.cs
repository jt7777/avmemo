using UnityEngine;
using System.Collections;

public class GameSettings : Singleton<GameSettings> {

	public const int InvalidTries  = 10000000;
	public const float InvalidTime = 10000000;

	public int memoryWidth = 5, memoryHeight = 4;

	public string HighscoreTriesName(int width, int height) {
		return "memory" + width + "x" + height + "_tries";
	}

	public string CurrentHighscoreTriesName() {
		return HighscoreTriesName(memoryWidth, memoryHeight);
	}
	
	public string HighscoreTimeName(int width, int height) {
		return "memory" + width + "x" + height + "_time";
	}
	
	public string CurrentHighscoreTimeName() {
		return HighscoreTimeName(memoryWidth, memoryHeight);
	}

	void Awake() {
		if(Assigned)
			Destroy (this);
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
}