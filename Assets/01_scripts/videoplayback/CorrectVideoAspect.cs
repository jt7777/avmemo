using UnityEngine;
using System.Collections;

public class CorrectVideoAspect : MonoBehaviour {

	public int width = 1280, height = 720;
	Vector2 rescale = Vector2.one, shift = Vector2.one;

	float lastQuadWidth, lastQuadHeight;
	Renderer r;
	Transform t;

	void Start() {
		r = GetComponent<Renderer>();
		t = GetComponent<Transform>();
	}

	// late update so the scaling happens after applying a video texture
	void LateUpdate () {
		// get local scale of this thing
		var quadWidth = Mathf.Abs (t.localScale.x);
		var quadHeight = Mathf.Max (Mathf.Abs (t.localScale.z), Mathf.Abs (t.localScale.y));

		if(quadWidth == lastQuadWidth && quadHeight == lastQuadHeight) return;

		lastQuadWidth = quadWidth;
		lastQuadHeight = quadHeight;

		var aImage = ((float)width)/height;
		var aQuad  = ((float)quadWidth)/quadHeight;

		if(height == 0 || width == 0 || quadHeight == 0 || quadWidth == 0) return;

		var a = aQuad / aImage;

		if(a < 1)
			rescale.Set (a,1);
		else
			rescale.Set (1,1/a);

		shift.Set((1-rescale.x)/2, (1-rescale.y)/2);

		var m = r.sharedMaterial;
		m.mainTextureOffset = shift;
		m.mainTextureScale = rescale;
	}
}