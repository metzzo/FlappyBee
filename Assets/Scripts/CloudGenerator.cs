using UnityEngine;
using System.Collections;

public class CloudGenerator : MonoBehaviour {
	public GameObject cloudPrefab;
	public int Threshold = 5;
	public Sprite[] Clouds;
	private float counter;

	void Start () {
		counter = 0;
		int count = 10;
		for (int i = 0; i < count; i++) {
			Spawn (i * (Camera.main.pixelWidth*4 / count));
		}
	}

	void Update () {
		var camX = Camera.main.transform.position.x;

		if (Mathf.Abs(camX - counter) > Threshold) {
			Spawn (Camera.main.pixelWidth * 2);

			counter = camX;
		}
	}

	void Spawn(float x) {
		var obj = Instantiate (cloudPrefab);
		var cam = Camera.main;
		var pos = cam.ScreenToWorldPoint (new Vector3 (x, cam.pixelHeight / 2, 45));
		pos.y += Random.Range (-40, 40);
		obj.transform.position = pos;
		obj.GetComponent<SpriteRenderer>().sprite = Clouds [Random.Range (0, Clouds.Length)];

		obj.transform.localScale *= Random.Range (0.5f, 1);
	}
}
