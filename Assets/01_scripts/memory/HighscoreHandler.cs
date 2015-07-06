using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighscoreHandler : MonoBehaviour {

//	public Text memory4x3_time, memory4x4_time, memory5x4_time;
//	public Text memory4x3_tries, memory4x4_tries, memory5x4_tries;
	public Text result_time, result_tries;

	public GameObject new_highscore, no_new_highscore;

	public Animator result;

	public void ShowResult(float time, int tries) {
		new_highscore.SetActive(false);
		no_new_highscore.SetActive(false);

		result_time.text = HighscoreShower.PrepareTime (time); // HACK format to min:sec
		result_tries.text = HighscoreShower.PrepareTries (tries);

		// read current highscore
		int currentHighscoreTries = PlayerPrefs.GetInt ( GameSettings.Instance.CurrentHighscoreTriesName() , GameSettings.InvalidTries);
		float currentHighscoreTime = PlayerPrefs.GetFloat ( GameSettings.Instance.CurrentHighscoreTimeName() , GameSettings.InvalidTime);

		// HACK if that is invalid, set it to lower zero for comparison
		//if(currentHighscoreTries >= GameSettings.InvalidTries)
		//	currentHighscoreTries = -1;

		if(tries < currentHighscoreTries || 
		   (tries == currentHighscoreTries && time < currentHighscoreTime))
		{
			new_highscore.SetActive(true);
		
			PlayerPrefs.SetInt (GameSettings.Instance.CurrentHighscoreTriesName(), tries);
			PlayerPrefs.SetFloat (GameSettings.Instance.CurrentHighscoreTimeName(), time);
			PlayerPrefs.Save ();
		}
		else
			no_new_highscore.SetActive(true);

		result.SetTrigger("Show");
	}
}
