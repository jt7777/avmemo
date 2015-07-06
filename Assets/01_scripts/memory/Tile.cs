using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public Grid parentGrid;
	public bool solved = false;

	public SpriteRenderer r;
	public TileData data;
	Animator animator;
	AudioSource audioClip;

	public Vector2 pos;

	IEnumerator Start() {
		if(data == null) yield break;

		animator = GetComponent<Animator>();
		audioClip = GetComponent<AudioSource>();

		yield return new WaitForSeconds(pos.y * 0.1f + pos.x * 0.1f);

		animator.SetTrigger("Appear");

		// r.color = data.color;
	}

	public void AssignData(TileData data) {
		this.data = data;
	}

	public void OnTileClick() {
		if(!solved)
			// check where we are in the statemachine!
			parentGrid.OnClickTile(this);
	}

	public void Show(bool playAudio = true) {
		animator.SetTrigger("Show");
		if(playAudio) {
			if(fadeOut != null) StopCoroutine(fadeOut);
			audioClip.volume = 1;
			audioClip.PlayOneShot(data.clip);
		}
	}

	public void Hide() {
		animator.SetTrigger("Hide");
	}

	public void HideNow() {
		animator.SetTrigger ("ToLevel");
	}

	public void Remove() {
		animator.SetTrigger("Remove");
		solved = true;
	}

	Coroutine fadeOut = null;
	public void FadeOut() {
		fadeOut = StartCoroutine (_FadeOut(0.5f));
	}

	IEnumerator _FadeOut(float duration) {
		var startVolume = audioClip.volume;

		// give the sound at least that time
		yield return new WaitForSeconds(0.7f);

		// now fade the sound out
		while(startVolume > 0) {
			startVolume -= Time.deltaTime / duration;
			audioClip.volume = startVolume;
			yield return null;
		}

		audioClip.volume = 0;
	}
}