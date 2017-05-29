using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {

	public SimpleObjectPool questionButtonObjectPool;
	public Transform questionButtonParent;
	public SimpleObjectPool answerButtonObjectPool;
	public Transform answerButtonParent;

	public GameObject questionDisplay;
	public GameObject roundOverDisplay;

	private DataController dataController;
	private RoundData currentRoundData;
	private QuestionData[] questionPool;

	private bool isRoundActive;
	private int questionIndex;
	private int playerScore;

	public Text questionTextObject;
	public string questionText;
	private string questionWord;
	private string questionWordCopy;

	private string answerWord;

	private int tempTilelimit;
	int numberOfTilesPopulated;

	private List<GameObject> questionButtonGameObjects = new List<GameObject>();
	private List<GameObject> answerButtonGameObjects = new List<GameObject>();

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
		numberOfTilesPopulated = 0;

		curateTileInformation ();

		ShowQuestion ();
		ShowAnswer ();
		isRoundActive = true;
	}

	// curateTileInformation(): will store required letters and the rest in an array
	private void curateTileInformation() {
		questionWord = questionPool [questionIndex].questionWord;
		questionWordCopy = questionWord;

		questionText = questionPool [questionIndex].questionText;
		questionTextObject.text = questionText;

		answerWord = questionPool [questionIndex].answers[0].answerText;
		answerWord = answerWord.ToUpper ();

		string randomString = RandomString ();
		questionWord = questionWord.ToUpper ();
		questionWord = questionWord + randomString;
		questionWord = shuffleTiles (questionWord);
	}

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
	}

	private string shuffleTiles(string unshuffled) {
		string shuffled = new string(unshuffled.OrderBy(r => Random.Range(0,9)).ToArray());

		return shuffled;
	}

	private void ShowQuestion() {
		RemoveQuestionButtons ();
		QuestionData questionData = questionPool [questionIndex];
		questionText = questionData.questionText;
		// GameObject questionButtonGameObject = null;
		// This is to populate the question buttons within the question-grid panel
		for (int i = 0; i < tempTilelimit; i++) {
			
				GameObject questionButtonGameObject = questionButtonObjectPool.GetObject ();
			
				questionButtonGameObjects.Add (questionButtonGameObject);
				questionButtonGameObject.transform.SetParent (questionButtonParent);

				QuestionButton questionButton = questionButtonGameObject.GetComponent<QuestionButton> ();
				questionButton.SetButtonIndex(i);
				questionButton.Setup (questionWord[i].ToString());
		}
	}
		
	private void RemoveQuestionButtons() {
		while (questionButtonGameObjects.Count > 0) {

			QuestionButton questionButtonSample = questionButtonGameObjects [0].GetComponent<QuestionButton> ();

			questionButtonObjectPool.ReturnObject (questionButtonGameObjects [0]);
			questionButtonGameObjects.RemoveAt (0);
		}

	}

	private void ShowAnswer() {
		RemoveAnswerButtons ();

		for (int i = 0; i < answerWord.Length; i++) {

				GameObject answerButtonGameObject = answerButtonObjectPool.GetObject ();

				answerButtonGameObjects.Add (answerButtonGameObject);
				answerButtonGameObject.transform.SetParent (answerButtonParent);

				AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton> ();
				answerButton.setButtonIndex (i);
				answerButton.Setup ("");
		}
	}
		
	private void RemoveAnswerButtons () {
		while (answerButtonGameObjects.Count > 0) {
			answerButtonObjectPool.ReturnObject (answerButtonGameObjects [0]);
			answerButtonGameObjects.RemoveAt (0);
		}
	}

	public void QuestionButtonClicked (string tileText, int mappedQuestionButtonIndex) {

		Debug.Log ("Question button clicked = "+mappedQuestionButtonIndex);
		
		foreach(GameObject ansBtn in answerButtonGameObjects) {
			AnswerButton answerButton = ansBtn.GetComponent<AnswerButton> ();

			if (answerButton.answerText.text == "") {
				answerButton.Setup (tileText);
				answerButton.SetMappedQuestionButtonIndex (mappedQuestionButtonIndex);

				checkIfAnswerIsComplete ();
				break;
			}
		}
	}

	public void HideUsedQuestionButton(int mappedQuestionButtonIndex) {
		foreach (GameObject quesBtn in questionButtonGameObjects) {
			QuestionButton questionButton = quesBtn.GetComponent<QuestionButton> ();
			if (questionButton.getButtonIndex() == mappedQuestionButtonIndex) {
				
				questionButton.SetButtonVisibility (false);
			}
		}
	}

	private void checkIfAnswerIsComplete() {
		numberOfTilesPopulated = 0;
		string enteredAnswer = "";

		foreach (GameObject ansBtn in answerButtonGameObjects) {
			AnswerButton answerButton = ansBtn.GetComponent<AnswerButton> ();

			if (answerButton.answerText.text != "") {
				numberOfTilesPopulated++;
				enteredAnswer += answerButton.answerText.text;

				if (numberOfTilesPopulated == answerWord.Length) {
					CheckCorrectnessOfAnswer (enteredAnswer);
				}
			}
		}
	}

	private void CheckCorrectnessOfAnswer(string enteredAnswer) {
		if (enteredAnswer.Equals (answerWord)) {
			Debug.Log ("answer is correct: " + enteredAnswer);

			IsNewQuestionAvailable ();

		} else {
			Debug.Log ("answer is incorrect : " + enteredAnswer);
		}
	}

	public void AnswerButtonClickCallback(int ansButtonIndex, int mappedQuestionButtonIndex) {

		Debug.Log ("answer button index = "+ansButtonIndex);

		foreach (GameObject quesBtn in questionButtonGameObjects) {
			QuestionButton questionButton = quesBtn.GetComponent<QuestionButton> ();
			if (questionButton.getButtonIndex() == mappedQuestionButtonIndex) {

				questionButton.SetButtonVisibility (true);
			}
		}
	}

	public void IsNewQuestionAvailable() {
		if (questionPool.Length > questionIndex + 1) {
			questionIndex++;

			DisplayNextQuestion ();

		} 
		else {
			EndRound ();
		}
	}

	private void DisplayNextQuestion() {
		prepareTilesForNewLevel ();
		curateTileInformation ();
		ShowQuestion ();
		ShowAnswer ();
	}

	// prepareTilesForNewLevel() is used to show all hidden/invisible tiles before new level starts
	private void prepareTilesForNewLevel() {
		foreach (GameObject quesBtn in questionButtonGameObjects) {
			QuestionButton questionButton = quesBtn.GetComponent<QuestionButton> ();
			questionButton.SetButtonVisibility (true);
		}
	}

	public void EndRound() {
		isRoundActive = false;

		questionDisplay.SetActive (false);
		roundOverDisplay.SetActive (true);
	}

	public void ReturnToMenu() {
		SceneManager.LoadScene ("MenuScene");
	}
	
	void Update () {
		
	}
}
