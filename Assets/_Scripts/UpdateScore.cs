using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
	public ScoreManager Score;

	private Text _text;

	private void Start()
	{
		_text = GetComponent<Text>();
	}

	private void Update ()
	{
		_text.text = Score.CurrentScore + "";
	}
}
