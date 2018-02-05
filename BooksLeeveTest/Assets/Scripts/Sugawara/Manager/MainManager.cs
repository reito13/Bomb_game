using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{
	private UIManager uiManager;
	private TimeManager timeManager;

	public static int playerNum = 1;

	private int i; //loop用

	public bool mainScene = true;

	private void Awake()
	{
		if (mainScene)
		{
			uiManager = GetComponent<UIManager>();
			timeManager = GetComponent<TimeManager>();

			StartCoroutine(MainCoroutine());
		}
	}

	private IEnumerator MainCoroutine()
	{
		yield return StartCoroutine(GameStart());

		yield return StartCoroutine(GameMainLoop());

		yield return StartCoroutine(GameEnd());
	}

	private IEnumerator GameStart()
	{
		for (i = 3;i>=0;i--)
		{
			uiManager.CountDown(i.ToString()); //カウントの文字を変更
			SoundManager.Instance.PlaySE("Count"); //カウントのSEを鳴らす
			yield return new WaitForSeconds(1.0f); //1秒待つ
		}
		uiManager.CountDown("GameStart");
		GameStatusManager.Instance.GameStart = false;
		yield return new WaitForSeconds(1.0f);
		uiManager.CountTextDelete();
	}

	private IEnumerator GameMainLoop()
	{
		while (!GameStatusManager.Instance.GameEnd) {
		
			yield return new WaitForSeconds(1.0f);
		}
	}
	
	public IEnumerator GameEnd()
	{
		SoundManager.Instance.PlaySE("GameEnd");

		GameStatusManager.Instance.GameEnd = true;
		uiManager.EndText();
		yield return new WaitForSeconds(1.0f);
		SceneTransitionManager.Instance.SceneTransition();
	}

	public void TimeUpdate()
	{
		uiManager.TimeUpdate(GetTime());
	}

	public int GetTime()
	{
		return timeManager.timeCount;
	}
}
