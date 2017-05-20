using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

	public Text questionText;
	private QuestionData questionData;
	private GameController gameController;
	private int questionButtonIndex;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}

	public void Setup(string data)
	{
		//questionData = data;
		questionText.text = data; //questionData.questionText;
	}

	public int getButtonIndex() {
		return questionButtonIndex;
	}

	public void SetButtonIndex(int index) {
		questionButtonIndex = index;
	}

	public void SetButtonVisibility(bool visibility) {
		if (!visibility) {
			this.GetComponent<Image> ().color = Color.clear;
			this.questionText.color = Color.clear;
		} 
		else {
			this.GetComponent<Image> ().color = Color.white;
			this.questionText.color = Color.black;
		}
	}

	public void HandleClick () {
		if (questionText.text != null) {
			SetButtonVisibility (false);
			gameController.QuestionButtonClicked (questionText.text, questionButtonIndex);
		} 
		else {
			gameController.QuestionButtonClicked ("", -1);
		}
	}
}
