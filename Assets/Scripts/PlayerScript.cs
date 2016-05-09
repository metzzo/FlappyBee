using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public struct Question {
	public string Text;
	public string CorrectText;
	public string IncorrectText;

	public Question(string text, string correctText, string incorrectText) {
		this.Text = text;
		this.CorrectText = correctText;
		this.IncorrectText = incorrectText;
	}

	public override string ToString ()
	{
		return "Text: " + Text + " " + CorrectText + " " + IncorrectText;
	}
}

public class PlayerScript : MonoBehaviour {
	private static Question[] questions = new Question[] {
		new Question("How many keys does a keyboard have?", "84 keys", "101 keys"),
		new Question("When was Google founded in September?", "4", "3"),
		new Question("When was the first video uploaded on youtube?", "April 23, 2005", "April 23, 2006"),
		new Question("What is the first name of a computer?", "ENIAC", "Mainframe"),
		new Question("1 TiB is ...", "1,048,576 MB", "1,000,000 MB"),
		new Question("8 Bit is...", "1 byte", "1 kb"),
		new Question("Basics colors of a screen are ...", "Red Green Blue", "Yellow Pink Black"),
		new Question("4K is ...", "2 times 1080p", "4 times 1080p"),
		new Question("What color represents facebook? ", "Blue", "Green"),
		new Question("Are ads useful to the user?", "No", "Yes"),
		new Question("What are cookies in computers?", "buiscuits", "data files"),
		new Question("What is DNS?", "Domain Name System", "Data Name System"),
		new Question("Quick Charge  technology now days is?", "3.0 A", "2.0 A"),
		new Question("Android at the beginning of 2016 launched the", "Android N", "Android M"),
		new Question("Linux is a open source operating system", "Yes", "No"),
		new Question("Which of the systems require more performance to run?", "Windows", "Linux"),
		new Question("The energy in a sound wave can be measured using ", "Decibels", "Megapixles"),
		new Question("Battery is measured  using", "mah", "meh"),
		new Question("Performance of a supercomputer is usually measured in", "FLOPS", "MIPS"),
		new Question("Internet speed measurement unit?", "Mbps", "Mp")
	};

	/// <summary>
	/// Currently playing Player
	/// </summary>
	public static PlayerScript CurrentPlayer;

	/// <summary>
	/// rigid body of this component
	/// </summary>
	private Rigidbody2D rigidBody;

	/// <summary>
	/// Speed of the movement to the right
	/// </summary>
	public float flySpeed = 10; 

	/// <summary>
	/// Speed of the jumping
	/// </summary>
	public float jumpSpeed = 1000;

	/// <summary>
	/// Stub which the camera Follows
	/// </summary>
	public GameObject FollowStub;

	/// <summary>
	/// The start position.
	/// </summary>
	private Vector3 startPosition;

	/// <summary>
	/// True if control is disabled
	/// </summary>
	private bool disableControl;

	/// <summary>
	/// Sprite GameObject
	/// </summary>
	private GameObject sprite;

	/// <summary>
	/// List of all questions
	/// </summary>
	private List<Question> shuffledQuestions;

	/// <summary>
	/// Current question
	/// </summary>
	private int currentQuestion;

	/// <summary>
	/// How many lifes does the player have?
	/// </summary>
	private int lifes;

	/// <summary>
	/// Score
	/// </summary>
	private int _score;
	public int Score {
		get {
			return _score;
		}
		set {
			if (value != _score) {
				_score = value;
				if (ScoreLabel != null) {
					ScoreLabel.GetComponent<Text>().text = "Score: " + _score;
				}
			}
		}
	}

	/// <summary>
	/// Score Label
	/// </summary>
	public GameObject ScoreLabel;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;

		rigidBody = GetComponent<Rigidbody2D> ();

		shuffledQuestions = new List<Question>(questions);

		Reset ();

		var cam = Camera.main;
		cam.transform.position = cam.ScreenToWorldPoint (new Vector3 (0, cam.pixelHeight / 2, -45));

		cam.transform.SetParent (FollowStub.transform);

		sprite = transform.FindChild ("PlayerSprite").gameObject;

		CurrentPlayer = this;

		lifes = 0;
	}

	void OnDestroy() {
		CurrentPlayer = null;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKeyDown (KeyCode.Space) || (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began)) && !disableControl) {
			rigidBody.AddForce(Vector2.up * jumpSpeed);
		}
		
		// check if out of screen
		var cam = Camera.main;
		var p = cam.WorldToScreenPoint (transform.position);
		if (p.y > cam.pixelHeight + 20 || p.y < -20) {
			Die ();
		}

		var stubPos = FollowStub.transform.position;
		stubPos.x = transform.position.x;
		FollowStub.transform.position = stubPos;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Obstacle" && !disableControl) {
			Debug.Log ("Disable Control! " + coll.gameObject.name);
			disableControl = true;
		}
	}

	void Die() {
		Debug.Log ("Die");

		// delete all Obstacles
		ObstacleGeneratorScript.CurrentObstacleGenerator.Reset ();

		Reset ();

		lifes--;
		if (lifes < 0) {
			MenuController.CurrentScore = Score;
			SceneManager.LoadScene ("Highscore");
		}
	}

	void Reset() {
		// reset player
		transform.position = startPosition;
		rigidBody.velocity = Vector2.right * flySpeed;
		rigidBody.angularVelocity = 0;
		disableControl = false;
		transform.eulerAngles = Vector3.zero;
		currentQuestion = 0;
		Shuffle (shuffledQuestions);
	}

	public void PassesThrough(bool isCorrect) {
		Debug.Log ("Passes thorugh " + isCorrect);
		if (isCorrect) {
			Score++;
		} else {
			disableControl = true;
		}
	}

	public Question NextQuestion() {
		Question q = shuffledQuestions [currentQuestion];
		currentQuestion = (currentQuestion + 1) % questions.Length;
		return q;
	}

	public static void Shuffle(IList<Question> list) {  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0, n + 1);  
			var value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
}
