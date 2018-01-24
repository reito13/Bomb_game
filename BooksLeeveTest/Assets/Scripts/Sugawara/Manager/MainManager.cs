using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{
	private UIManager uiManager;
	private ScoreManager scoreManager;

	private new Renderer renderer;

	public static int playerNum = 2;

	public bool mainScene = true;

	private void Awake()
	{
		if (mainScene)
		{
			uiManager = GetComponent<UIManager>();
			scoreManager = GetComponent<ScoreManager>();
			scoreManager.ResetScore();
			StartCoroutine(GameStart());
		}
	}

	private IEnumerator GameStart()
	{
		uiManager.CountDown("3");
		SoundManager.Instance.PlaySE("Count");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("2");
		SoundManager.Instance.PlaySE("Count");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("1");
		SoundManager.Instance.PlaySE("Count");
		yield return new WaitForSeconds(1.0f);
		uiManager.CountDown("GameStart");
		SoundManager.Instance.PlaySE("GameStart");

		GameStatusManager.Instance.GameStart = false;
		yield return new WaitForSeconds(1.0f);
		uiManager.CountTextDelete();
	}
	
	public IEnumerator GameEnd()
	{
		SoundManager.Instance.PlaySE("GameEnd");

		GameStatusManager.Instance.GameEnd = true;
		uiManager.EndText();
		yield return new WaitForSeconds(1.0f);
		SceneTransitionManager.Instance.SceneTransition();
	}

	public void TimeUpdate(int timeCount)
	{
		uiManager.TimeUpdate(timeCount);
	}

	public void AddScore(int num)
	{
		scoreManager.AddScore(num);
		uiManager.ScoreUpdate(num,scoreManager.GetScore(num));
	}

	public void BombUpdate(int value)
	{
		uiManager.BombUpdate(value);
	}

	public IEnumerator TakeScore(int num,bool fall,float delayTime)
	{
		yield return new WaitForSeconds(delayTime);

		if (fall)
		{
			scoreManager.DivideScore(num);
		}
		else
		{
			scoreManager.TakeScore(num,20);
		}
		uiManager.ScoreUpdate(num, scoreManager.GetScore(num));
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
