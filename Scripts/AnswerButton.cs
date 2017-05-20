using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerButton : MonoBehaviour {

	public Text answerText;
	private AnswerData answerData;
	private GameController gameController = null;
	private int buttonIndex;
	private int mappedQuestionButtonIndex;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}

	public void Setup(string data)
	{
		answerText.text = data;
	}

	public void setButtonIndex(int index) {
		buttonIndex = index;
	}

	public int getButtonIndex() {
		return this.buttonIndex;
	}

	public int GetMappedQuestionButtonIndex(){
		return mappedQuestionButtonIndex;
	}

	public void SetMappedQuestionButtonIndex(int index){
		mappedQuestionButtonIndex = index;
	}

	public void HandleClick () {
		if (answerText.text != null) {
			answerText.text = "";
			gameController.AnswerButtonClickCallback (buttonIndex, mappedQuestionButtonIndex);
		} else {
			// do nothing for now
		}
	}
}
