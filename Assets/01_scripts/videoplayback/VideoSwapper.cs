using UnityEngine;
using System.Collections;

public class VideoSwapper : MonoBehaviour {
	
	public VideoTexture_Material vtm;

	public void PlayMovie(Texture2DMovie mt) {
		vtm.Stop ();

		vtm.FileName = mt.fileName;
		vtm.firstFrame = mt.firstFrame;
		vtm.lastFrame = mt.lastFrame;
		vtm.digitsFormat = mt.digitsFormat;
		vtm.enableAudio = true;
		vtm.GetComponent<AudioSource>().clip = mt.clip;
		vtm.Reset();

		vtm.Play ();
	}
}