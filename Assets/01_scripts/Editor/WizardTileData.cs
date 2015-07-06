using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;

public class WizardTileData : ScriptableWizard
{
	public AudioClip[] clips;
	public TileData prefab;

	[MenuItem ("GameObject/Create TileData Wizard")]
	static void CreateWizard () {
		ScriptableWizard.DisplayWizard<WizardTileData>("Create TileData", "Create");
	}
	
	void OnWizardCreate () {
		// take all the audio clips applied here
		for(int i = 0; i < clips.Length; i++) {
			// create TileData gameobjects for them
			// adapt all the settings


			var clipName = clips[i].name;

			// look in resources folder how many frames there are
			var path = Application.dataPath + "/" + "02_assets/videos/Resources/" + clipName;
			var d = new DirectoryInfo(path);

			var p = Instantiate<TileData>(prefab);
			p.clip = clips[i];
			p.color = Color.white;
			p.movie = new Texture2DMovie();
			p.movie.clip = clips[i];

			var files = d.GetFiles ("*.jpg");
			p.movie.firstFrame = 0;
			p.movie.lastFrame = files.Length - 1;

			// HACK works up to 999 frames
			p.movie.digitsFormat = "0";
			if(files.Length >= 10)
				p.movie.digitsFormat = "00";
			if(files.Length >= 100)
				p.movie.digitsFormat = "000";

			p.name = clipName;
		}
	}
	
	void OnWizardUpdate () {
		helpString = "Please set the color of the light!";
	}
	
//	// When the user pressed the "Apply" button OnWizardOtherButton is called.
//	void OnWizardOtherButton () {
//		if (Selection.activeTransform != null) {
//			Light lt = Selection.activeTransform.GetComponent<Light>();
//			
//			if (lt != null) {
//				lt.color = Color.red;
//			}
//		}
//	}
}