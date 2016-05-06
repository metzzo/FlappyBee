using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

	/// <summary>
	/// Barrier at the top
	/// </summary>
	private GameObject topBarrier;

	/// <summary>
	/// Barrier in the middle
	/// </summary>
	private GameObject middleBarrier;

	/// <summary>
	/// Barrier at the bottom
	/// </summary>
	private GameObject bottomBarrier;

	/// <summary>
	/// Variation of position
	/// </summary>
	public float Variation = 5.0f;

	/// <summary>
	/// The overall offset
	/// </summary>
	public float BiasVariation = 10;

	void Start () {
		topBarrier = transform.FindChild ("TopBarrier").gameObject;
		middleBarrier = transform.FindChild ("MiddleBarrier").gameObject;
		bottomBarrier = transform.FindChild ("BottomBarrier").gameObject;

		var bias = Random.Range (-BiasVariation, BiasVariation);

		topBarrier.transform.Translate (new Vector3 (0, bias, 0));
		middleBarrier.transform.Translate (new Vector3 (0, bias, 0));
		bottomBarrier.transform.Translate (new Vector3 (0, bias, 0));

		var q = PlayerScript.CurrentPlayer.NextQuestion ();
		var topPassage = middleBarrier.transform.FindChild ("TopPassage").GetComponent<PassageScript>();
		var bottomPassage = middleBarrier.transform.FindChild ("BottomPassage").GetComponent<PassageScript>();
		var question = transform.FindChild ("Question").GetComponent<TextMesh> ();
		question.text = ResolveTextSize(q.Text, 20);

		if (Random.Range (0, 2) == 0) {
			topPassage.IsCorrect = false;
			topPassage.Text = q.CorrectText;
			
			bottomPassage.IsCorrect = true;
			bottomPassage.Text = q.IncorrectText;
		} else {
			topPassage.IsCorrect = true;
			topPassage.Text = q.IncorrectText;
			
			bottomPassage.IsCorrect = false;
			bottomPassage.Text = q.CorrectText;
		}

		Debug.Log (q);
	}

	// Wrap text by line height
	private string ResolveTextSize(string input, int lineLength){
		string[] words = input.Split(" "[0]);
		string result = "";
		string line = "";
		foreach(string s in words){
			string temp = line + " " + s;
			if(temp.Length > lineLength){
				result += line + "\n";
				line = s;
			} else {
				line = temp;
			}
		}
		result += line;
		return result.Substring(1,result.Length-1);
	}
	
	void Update () {
		
		//topBarrier.transform.Rotate (0, Random.Range (-Variation, Variation), 0);
		//middleBarrier.transform.Rotate (0, Random.Range (-Variation, Variation), 0);
		//bottomBarrier.transform.Rotate (0, Random.Range (-Variation, Variation), 0);
	}
}
