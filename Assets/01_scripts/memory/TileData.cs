using UnityEngine;
using System.Collections;

public class TileData : MonoBehaviour {
	[HideInInspector]
	public Color color;
	public AudioClip clip;
	public Texture2DMovie movie;
	
	public static bool Matches(TileData a, TileData b) {
		return a.clip == b.clip;
	}
}