using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour {

	public RoundData[] allRoundData;

	void Start () {
		DontDestroyOnLoad (gameObject);
		SceneManager.LoadScene ("MenuScene");
	}

	public RoundData GetCurrentRoundData() {
		return allRoundData [0];
	}
	
	void Update () {
		
	}
}
