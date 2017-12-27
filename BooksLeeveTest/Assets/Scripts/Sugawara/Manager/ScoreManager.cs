using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

	public static int[] scores = { 0, 0 };

	public void ResetScore()
	{
		scores[0] = 0;
		scores[1] = 0;
	}

	public void AddScore(int num)
	{
		num--;
		scores[num]++;
	}

	public void TakeScore(int num, int value)
	{
		num--;
		scores[num] -= value;

		if (scores[num] <= 0)
		{
			scores[num] = 0;
		}
	}

	public void DivideScore(int num)
	{
		num--;
		scores[num] /= 2;
	}

	public int GetScore(int num)
	{
		return scores[num-1];
	}
}
