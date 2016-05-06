using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class MenuController : MonoBehaviour {
	[Serializable]
	public class Highscore {
		public Score[] highscore;
	}

	[Serializable]
	public class Score {
		public string name;
		public int score;
	}
	private Highscore highscore;

	public GameObject HighscoreEntryTemplate;
	private GameObject ListPanel;

	IEnumerator Start () {
		ListPanel = transform.Find ("Canvas/ListPanel").gameObject;

		WWW www = new WWW ("http://programming-with-design.at/flappybee/highscore.php");
		yield return www;

		var text = www.text;

		highscore = JsonUtility.FromJson<Highscore> (text);

		BuildHighscore();
	}

	void Update () {
	
	}

	void BuildHighscore() {
		var pos = -130;
		foreach (var score in highscore.highscore) {
			var newObj = Instantiate(HighscoreEntryTemplate);
			newObj.transform.SetParent (ListPanel.transform, false);

			newObj.transform.FindChild("Name").GetComponent<Text>().text = score.name;
			newObj.transform.FindChild("Score").GetComponent<Text>().text = "" + score.score;

			var vec = newObj.GetComponent<RectTransform>().position;
			vec.y += pos;
			newObj.GetComponent<RectTransform>().position = vec;



			pos += 80;
		}
	}
}
