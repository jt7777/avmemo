using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VideoPlayer : MonoBehaviour {

	// we need some groups for the different memory layouts
	// 1x1 (1 video, 1 simulatneously)
	// 3x2 (3 videos, 3 simultaneously)
	// 4x3 (6 videos, 4 simultaneously)
	// 4x4 (8 videos, 4 simultaneously)
	// 5x4 (10 videos, 4 simultaneously)

	public Animator singleVideo;
	public VideoSwapper singleVideoPlayer;

	public Animator multiVideo;
	public VideoSwapper[] multiVideoPlayer;

	public Animator singleVideoFullscreen;
	public VideoSwapper singleVideoFullscreenPlayer;

	public void Awake() {
		singleVideo.gameObject.SetActive(false);
	}

	public IEnumerator Play(Texture2DMovie movie) {
		singleVideo.gameObject.SetActive(true);
		singleVideoPlayer.PlayMovie (movie);
		yield return new WaitForSeconds(movie.length);
		singleVideo.gameObject.SetActive(false);
	}

	public IEnumerator PlayPiece(Texture2DMovie movie) {
		singleVideoFullscreen.gameObject.SetActive(true);

		var duration = Mathf.Min (SelectRandomLength(), movie.length);
		
		singleVideoFullscreenPlayer.PlayMovie (movie);
		yield return new WaitForSeconds(duration);

		// this is an eights
		if(duration <= lengths[0])
		{
			singleVideoFullscreenPlayer.PlayMovie (movie);
			yield return new WaitForSeconds(duration);
		}

		singleVideoFullscreen.gameObject.SetActive(false);
	}

	// random sound player
	float[] lengths = new float[] { 0.5f, 0.5f, 0.5f, 0.5f, 1.0f, 1.0f, 2.0f, 4.0f};
	float SelectRandomLength() {
		return lengths[Random.Range(0, lengths.Length)];
	}

	int currentIndex;
	List<Texture2DMovie> shuffledPlaylist;

	// this is the multivideo player
	public IEnumerator Play(Texture2DMovie[] movies) {
		var count = Mathf.Min (movies.Length, multiVideoPlayer.Length);
		multiVideo.gameObject.SetActive(true);

		shuffledPlaylist = new List<Texture2DMovie>(movies);
		shuffledPlaylist.Shuffle();

		currentIndex = 0;

		for(int i = 0; i < multiVideoPlayer.Length; i++) {
			StartCoroutine (PlayVideo(multiVideoPlayer[i], shuffledPlaylist[currentIndex++], movies));
		}

		yield return new WaitForSeconds(12);
		StopAllCoroutines();
		multiVideo.gameObject.SetActive(false);
	}

	// this is the player for playing all videos one after another
	public IEnumerator PlayList(Texture2DMovie[] movies) {
		var startTime = Time.time;
		var playTime = 16.0f;

		int i = 0;

		// for(int i = 0; i < movies.Length; i++) {
		while(Time.time - startTime < playTime) {
			i++;
			yield return StartCoroutine(PlayPiece (movies[i % movies.Length]));
		}
	}

	IEnumerator PlayVideo(VideoSwapper player, Texture2DMovie movie, Texture2DMovie[] movieList) {
		// play this video
		int playCount = 0;
		int everyNthSwitchConfiguration = 4;
		while(true) {
			player.PlayMovie(movie);
			yield return new WaitForSeconds(movie.length);
			playCount++;
			if(playCount % everyNthSwitchConfiguration == 0)
			{
				multiVideo.SetInteger("RandomSelector", Random.Range(0,2));
				multiVideo.SetTrigger("Next");
			}
			// next video in playlist
			currentIndex = (currentIndex + 1) % shuffledPlaylist.Count;
			movie = shuffledPlaylist[currentIndex];
		}
	}
}
