using UnityEngine;
using System.Collections;

public class ObstacleDestroyerScript : MonoBehaviour {
	void Start () {
	
	}

	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Obstacle") {
			Debug.Log ("Destroy! " + coll.gameObject.name);
			Destroy (coll.transform.parent.gameObject);
		}
	}
}
