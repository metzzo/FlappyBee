using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public static int CurrentScore = -1;
	private static string URL = "http://programming-with-design.at/flappybee/highscore.php";

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
	private GameObject EnterHighscore;
	private InputField NameField;
	private Button ButtonField;

	private List<GameObject> ListEntries;

	void Start () {
		ListPanel = transform.Find ("Canvas/ListPanel").gameObject;
		EnterHighscore = GameObject.Find ("EnterHighscore");
		NameField = GameObject.Find ("InputField").GetComponent<InputField>();
		ButtonField = GameObject.Find ("SubmitButton").GetComponent<Button> ();
		EnterHighscore.SetActive (false);

		ListEntries = new List<GameObject> ();
	
		StartCoroutine ("FetchHighscore");
	}

	IEnumerator FetchHighscore() {
		WWW www = new WWW (URL);
		yield return www;
		
		var text = www.text;
		
		highscore = JsonUtility.FromJson<Highscore> (text);
		
		BuildHighscore();

		if (!EnterHighscore.activeInHierarchy) {
			if (CurrentScore >= 0) {
				if (highscore.highscore.Length < 10) {
					EnterHighscore.SetActive (true);
				} else {
					foreach (var score in highscore.highscore) {
						if (score.score < CurrentScore) {
							EnterHighscore.SetActive (true);
							break;
						}
					}
				}
			}
		}
	}

	void BuildHighscore() {
		foreach (var entry in ListEntries) {
			Destroy (entry);
		}

		var panel = ListPanel.transform.Find ("ContentPanel");
		foreach (var score in highscore.highscore) {
			var newObj = Instantiate(HighscoreEntryTemplate);
			newObj.transform.SetParent (panel, false);

			newObj.transform.FindChild("Name").GetComponent<Text>().text = score.name;
			newObj.transform.FindChild("Score").GetComponent<Text>().text = "" + score.score;
			ListEntries.Add (newObj);

		}
	}

	public void PlayGame() {
		GetComponent<AudioSource> ().Play ();

		SceneManager.LoadScene ("Main");
	}

	void SubmitHighscore() {
		GetComponent<AudioSource> ().Play ();

		StartCoroutine ("DoSubmitHighscore");
	}

	IEnumerator DoSubmitHighscore() {
		if (CurrentScore > -1) {
			var form = new WWWForm ();
			form.AddField ("score", CurrentScore);
			form.AddField ("name", NameField.text);
			WWW www = new WWW (URL, form);
			yield return www;

			StartCoroutine ("FetchHighscore");

			CurrentScore = -1;
		}
		EnterHighscore.SetActive (false);
		ButtonField.interactable = false;
	}

	public void NameChanged() {
		Debug.Log (NameField.text);

		ButtonField.interactable = NameField.text.Length > 3;
	}
}
