using UnityEngine;
using System.Collections;

public class PassageScript : MonoBehaviour {
	public bool IsCorrect;
	private string _text;
	public string Text {
		get {
			return _text;
		}
		set {
			if (value != _text) {
				_text = value;
				transform.FindChild("Text").GetComponent<TextMesh>().text = _text;
			}
		}
	}

	void Start () {
		
	}

	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		var player = coll.gameObject.GetComponent<PlayerScript> ();
		if (player != null) {
			player.PassesThrough(IsCorrect);
			Debug.Log (coll.gameObject.name);
		}
	}
}
