using UnityEngine;
using System.Collections;

[System.Serializable]
public class Texture2DMovie
{
	public AudioClip clip;
	[HideInInspector]
	public Texture2D texture;
	public float length {
		get {
			return (lastFrame - firstFrame)/25.0f; 
		}
	}

	public int firstFrame, lastFrame;
	// public string fileName;
	// HACK for correctly named files - this should be the same as the audio clip name
	public string fileName {
		get {
			return clip.name + "/" + clip.name;
		}
	}
	public string digitsFormat = "00";
}
