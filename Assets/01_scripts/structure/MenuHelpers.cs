using UnityEngine;
using System.Collections;

public class MenuHelpers : MonoBehaviour {

	public bool preloadMemory = false;
	public Grid grid;

	AsyncOperation asyncMemoryLoad;

	IEnumerator Start() {
		if(!preloadMemory) yield break;

		yield return new WaitForSeconds(2.0f);

		asyncMemoryLoad = Application.LoadLevelAsync("memory");
		asyncMemoryLoad.allowSceneActivation = false;
		asyncMemoryLoad.priority = (int)ThreadPriority.Low;
//		StartCoroutine(_ShowProgress());
		yield return asyncMemoryLoad;
	}

//	IEnumerator _ShowProgress() {
//		while(asyncMemoryLoad.progress <= 0.9f)
//		{
//			print ("loading: " + asyncMemoryLoad.progress * 100 + "%");
//			yield return null;
//		}
//
//		print ("loading: 100%");
//	}

	void Update() {
		if(Input.GetKeyDown (KeyCode.Escape)) {
			BackToMenu();
		}
	}

	public void BackToMenu() {
		StartCoroutine(_BackToMenu());
	}

	IEnumerator _BackToMenu() {
		GetComponent<Animator>().SetTrigger("ToLevel");
		grid.HideAllTiles();

		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel ("menu");
	}

	public void SetWidth(int width) {
		GameSettings.Instance.memoryWidth = width;
	}
	
	public void SetHeight(int height) {
		GameSettings.Instance.memoryHeight = height;
	}
	
	public void LoadLevel(string name) {
		StartCoroutine(_LoadLevel(name));
	}

	IEnumerator _LoadLevel(string name) {
		GetComponent<Animator>().SetTrigger("ToLevel");
		yield return new WaitForSeconds(1.2f);

		ResourceCache.UnloadUnusedAssets();

		if(name == "memory")
			asyncMemoryLoad.allowSceneActivation = true;
		else
			Application.LoadLevel (name);
	}

	public void Quit() {
		Application.Quit();
	}

	public void OpenURL(string url) {
		Application.OpenURL(url);
	}
}