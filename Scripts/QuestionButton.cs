using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class QuestionButton : MonoBehaviour {

	public Text questionText;
	private QuestionData questionData;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = FindObjectOfType<GameController> ();
	}

	public void Setup(string data)
	{
		//questionData = data;
		questionText.text = data; //questionData.questionText;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
