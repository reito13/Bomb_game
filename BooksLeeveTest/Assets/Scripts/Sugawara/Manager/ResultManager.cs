using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
	[SerializeField] private Text[] scoreTexts = null;

	private void Start()
	{
		SoundManager.Instance.PlaySE("Result");
		scoreTexts[0].text = ScoreManager.scores[0].ToString() + "P";
		scoreTexts[1].text = ScoreManager.scores[1].ToString() + "P";
	}

	void Update()
	{
		if (Input.anyKeyDown)
		{
			SceneTransitionManager.Instance.SceneTransition();
		}
	}
}
