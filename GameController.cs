using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

	public SimpleObjectPool questionButtonObjectPool;
	public Transform questionButtonParent;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private int questionIndex;
	private int playerScore;

	public Text questionText;
	private string questionWord;
	private string questionWordCopy;

	private int tempTilelimit;
	//private Text[] tileData;

	private List<GameObject> questionButtonGameObjects = new List<GameObject>();

	void Start () {
		questionText = null;
		questionWord = null;
		questionWordCopy = null;

		dataController = FindObjectOfType<DataController> ();
		currentRoundData = dataController.GetCurrentRoundData ();
		questionPool = currentRoundData.questions;

		playerScore = 0;
		questionIndex = 0;
		tempTilelimit = 9;
		curateTileInformation ();

		ShowQuestion ();
		isRoundActive = true;
	}

	// curateTileInformation(): will store required letters and the rest in an array
	private void curateTileInformation() {
		questionWord = questionPool [questionIndex].questionWord;
		questionWordCopy = questionWord;
		string randomString = RandomString();

		questionWord = questionWord.ToUpper ();
		questionWord = questionWord + randomString;
		questionWord = shuffleTiles (questionWord);
	}

	private static Random random = new Random();
	public string RandomString()
	{
		string myString = "";
		const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int randomStringLength = tempTilelimit - questionWord.Length;
		int charAmount = Random.Range(randomStringLength, randomStringLength);
		for(int i=0; i<charAmount; i++)
		{
			myString += glyphs[Random.Range(0, glyphs.Length)];
		}
		return myString;

		//return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
	}

	private string shuffleTiles(string unshuffled) {
		var rnd = new Random();
		string shuffled = new string(unshuffled.OrderBy(r => Random.Range(0,9)).ToArray());

		return shuffled;
	}

	private void ShowQuestion() {
		RemoveQuestionButtons ();
		//QuestionData questionData = questionPool [questionIndex];
		//questionText.text = questionData.questionText;
		GameObject questionButtonGameObject = null;
		// This is to populate the question buttons within the question-grid panel
		for (int i = 0; i < tempTilelimit; i++) {
			
			if (questionButtonObjectPool) {
				questionButtonGameObject = questionButtonObjectPool.GetObject ();
			
				questionButtonGameObjects.Add (questionButtonGameObject);
				questionButtonGameObject.transform.SetParent (questionButtonParent);

				QuestionButton questionButton = questionButtonGameObject.GetComponent<QuestionButton> ();
				questionButton.Setup (questionWord[i].ToString());
			}
		}
	}

	private void ShowAnswer() {
		for (int i = 0; i < questionWordCopy.Length; i++) {
		
		}
	}

	private void RemoveQuestionButtons() {
		while (questionButtonGameObjects.Count > 0) {
			questionButtonObjectPool.ReturnObject (questionButtonGameObjects [0]);
			questionButtonGameObjects.RemoveAt (0);
		}
	}
	
	void Update () {
		
	}
}
