using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreShower : MonoBehaviour {
	
	public Text memory4x3_time, memory4x4_time, memory5x4_time;
	public Text memory4x3_tries, memory4x4_tries, memory5x4_tries;

	// Use this for initialization
	void Start () {
		// read highscore on start
		var readFloat = 0.0f;
		var readInt = 0;

		ChangeTimeText (memory4x3_time, GameSettings.Instance.HighscoreTimeName(4,3));
		ChangeTimeText (memory4x4_time, GameSettings.Instance.HighscoreTimeName(4,4));
		ChangeTimeText (memory5x4_time, GameSettings.Instance.HighscoreTimeName(5,4));

		ChangeTriesText (memory4x3_tries, GameSettings.Instance.HighscoreTriesName(4,3));
		ChangeTriesText (memory4x4_tries, GameSettings.Instance.HighscoreTriesName(4,4));
		ChangeTriesText (memory5x4_tries, GameSettings.Instance.HighscoreTriesName(5,4));
	}

	void ChangeTimeText(Text t, string fieldName) {
		t.text = PrepareTime(PlayerPrefs.GetFloat (fieldName, GameSettings.InvalidTime));
	}

	void ChangeTriesText(Text t, string fieldName) {
		t.text = PrepareTries(PlayerPrefs.GetInt (fieldName, GameSettings.InvalidTries));
	}

	public static string PrepareTime(float time) {
		if(time >= GameSettings.InvalidTime - 0.1f)
			return  "-";
		else {
			int seconds = (int)time;
			return string.Format("{0}:{1:00}", seconds / 60, seconds % 60) + " min";
		}
	}

	public static string PrepareTries(int tries) {
		if(tries >= GameSettings.InvalidTries)
			return  "-";
		else {
			return tries + " tries";
		}
	}

	public void ResetHighscore() {
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTimeName(4,3));
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTimeName(4,4));
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTimeName(5,4));
		
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTriesName(4,3));
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTriesName(4,4));
		PlayerPrefs.DeleteKey(GameSettings.Instance.HighscoreTriesName(5,4));

		PlayerPrefs.Save ();

		Start();
	}
}
