using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
	private GameObject bg1, bg2;
	private bool first = false;

	public Sprite[] sprites;

	void Start () {
		bg1 = transform.FindChild ("Background1").gameObject;
		bg2 = transform.FindChild ("Background2").gameObject;

		bg1.GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length)];
		bg2.GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length)];

		bg1.GetComponent<AutoStretchSprite> ().Resize ();
		bg2.GetComponent<AutoStretchSprite> ().Resize ();

		Reposition ();
	}
	  
	void Update () {
		if (!bg1.GetComponent<Renderer> ().isVisible) {
			if (!first) {
				first = true;
			} else {
				Debug.Log ("Not visible");
				var tmp = bg1;
				bg1 = bg2;
				bg2 = tmp;

				Reposition ();
			}
		}
	}

	void Reposition() {
		var width = bg1.GetComponent<SpriteRenderer>().sprite.bounds.size.x * bg1.transform.localScale.x;

		bg2.transform.position = bg1.transform.position + new Vector3(width, 0, 0);


	}
}
