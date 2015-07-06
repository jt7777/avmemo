using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceCache : Singleton<ResourceCache> {

	public static Dictionary<string, Object> cache = new Dictionary<string, Object>();

	public static Object Load(string path) {
		Object o;
		if(cache.TryGetValue(path, out o)) {
			return o;
		}
		else {
			o = Resources.Load (path);
			cache.Add (path, o);
			return o;
		}
	}

	public static void UnloadAsset(Object o) {
		string key = "";
		foreach(var kvp in cache) {
			if(kvp.Value == o) {
				key = kvp.Key;
				break;
			}
		}

		Resources.UnloadAsset(o);
		if(cache.ContainsKey(key))
			cache.Remove (key);
	}

	public static void UnloadUnusedAssets() {
		Resources.UnloadUnusedAssets();
		cache.Clear ();
	}
}
