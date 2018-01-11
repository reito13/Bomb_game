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

	[SerializeField] private Image[] images = null;

	private int i = 0;

	public void TimeUpdate(int timeCount)
	{
		timeText.text = timeCount.ToString();
	}

	public void ScoreUpdate(int num,int score)
	{
		scoreTexts[num-1].text = score.ToString() + "P";
	}

	public void BombUpdate(int value)
	{
		for(i = 0; i < 3; i++)
		{
			if (i + 1 > value)
				images[i].enabled = false;
			else
				images[i].enabled = true;
		}
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
