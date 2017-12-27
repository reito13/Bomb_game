using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private Text timeText = null;
	[SerializeField] private Text countDownText = null;
	[SerializeField] private Text endText = null;

	[SerializeField] private Text[] scoreTexts = null;

	public void TimeUpdate(int timeCount)
	{
		timeText.text = timeCount.ToString();
	}

	public void ScoreUpdate(int num,int score)
	{
		scoreTexts[num].text = score.ToString() + "P";
	}

	public void CountDown(string countText)
	{
		countDownText.text = countText;
	}
	public void CountTextDelete()
	{
		countDownText.enabled = false;
	}

	public void EndText()
	{
		endText.enabled = true;
	}
}
