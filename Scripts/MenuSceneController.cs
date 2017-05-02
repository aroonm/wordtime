using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour {

	public void StartGame() {
		SceneManager.LoadScene("Game");
	}
}
