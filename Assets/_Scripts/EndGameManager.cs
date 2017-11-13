using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{

	public ScoreManager ScoreManager;
	public GameObject FinalScore;
	public GameObject HighScore;

	private void Start ()
	{
		FinalScore.GetComponent<Text>().text = ScoreManager.CurrentScore + "";
		HighScore.GetComponent<Text>().text = ScoreManager.HighScore + "";
	}

	public void GoBack()
	{
        SceneManager.LoadScene("MainMenu");
	}
}
