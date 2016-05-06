using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleGeneratorScript : MonoBehaviour {
	/// <summary>
	/// Current Obstacle Generator Script
	/// </summary>
	public static ObstacleGeneratorScript CurrentObstacleGenerator;

	/// <summary>
	/// How many Updates should happen till a new obstacle is spawned?
	/// </summary>
	public int SpawnCounter = 100;

	/// <summary>
	/// With how much variation to the spawning?
	/// </summary>
	public int SpawnVariation = 50;

	/// <summary>
	/// Thrshold at the beginning
	/// </summary>
	public int InitialCounterThreshold = 100;

	/// <summary>
	/// Which obstacle should be instantiated?
	/// </summary>
	public GameObject obstacle;

	/// <summary>
	/// Whats the current counter
	/// </summary>
	private float counter;

	/// <summary>
	/// At which threshold should a new obstacle be generated
	/// </summary>
	private int counterThreshold;

	/// <summary>
	/// All Obstacles that this script has initialized and are still living
	/// </summary>
	private List<GameObject> Obstacles;

	void Start () {
		counterThreshold = InitialCounterThreshold;
		counter = 0;

		Obstacles = new List<GameObject> ();

		CurrentObstacleGenerator = this;
	}

	void Update () {
		var camX = Camera.main.transform.position.x;

		if ((camX - counter) > counterThreshold) {
			Debug.Log ("Spawn");

			var obj = Instantiate(obstacle);
			Obstacles.Add(obj);

			var cam = Camera.main;
			var pos = cam.ScreenToWorldPoint(new Vector3(Screen.width*2, cam.pixelHeight/2, 45));
			pos.z = 0;
			obj.transform.position = pos;

			counterThreshold = SpawnCounter + Random.Range (-SpawnVariation, SpawnVariation);
			counter = camX;
		}
	}

	void OnDestroy() {
		CurrentObstacleGenerator = null;
	}

	public void Reset() {
		foreach (var obj in Obstacles) {
			Destroy (obj);
		}
		Obstacles.Clear ();

		counterThreshold = InitialCounterThreshold;
		counter = 0;
	}
}
