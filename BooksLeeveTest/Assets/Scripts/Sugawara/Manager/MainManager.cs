using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{
	private UIManager uiManager;

	private int[] scores = { 0, 0 };

	private new Renderer renderer;

	private void Awake()
	{
		uiManager = GetComponent<UIManager>();
		StartCoroutine(GameStart());
	}

	private IEnumerator GameStart()
	{
		uiManager.CountDown("3");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("2");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("1");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("GameStart");

		GameStatusManager.Instance.GameStart = false;
		yield return new WaitForSeconds(1.0f);
		uiManager.CountTextDelete();
	}
	
	public void GameEnd()
	{
		GameStatusManager.Instance.GameEnd = true;
		uiManager.EndText();
	}

	public void TimeUpdate(int timeCount)
	{
		uiManager.TimeUpdate(timeCount);
	}

	public void AddScore(int num)
	{
		num--;
		scores[num]++;
		uiManager.ScoreUpdate(num,scores[num]);
	}

	public IEnumerator TakeScore(int num,bool fall,float delayTime)
	{
		num--;
		yield return new WaitForSeconds(delayTime);

		if (fall)
		{
			scores[num] /= 2;
		}
		else
		{
			scores[num] -= 20;
			if (scores[num] <= 0)
				scores[num] = 0;
		}
		uiManager.ScoreUpdate(num,scores[num]);
	}

	public void StageDelete(Collider collider)
	{
		collider.enabled = false;
		renderer = collider.GetComponent<Renderer>();
		renderer.enabled = false;
		//StartCoroutine(StageRepair(collider,renderer));
	}

	private IEnumerator StageRepair(Collider coll, Renderer ren)
	{
		yield return new WaitForSeconds(5.0f);
		coll.enabled = true;
		ren.enabled = true;
	}
}
