using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{
	private UIManager uiManager;
	private TimeManager timeManager;

	public static int playerNum = 1;

	[SerializeField] FadeController fadeController = null;
	private int i; //loop用

	public bool mainScene = true;

	private int count = 0;

	public bool[] playerGameOver = new bool[4];

	[SerializeField] private PhotonManager photonManager = null;

	private void Awake()
	{
		if (mainScene)
		{
			uiManager = GetComponent<UIManager>();
			timeManager = GetComponent<TimeManager>();

			//StartCoroutine(MainCoroutine());
		}

		for(i = 0; i < playerGameOver.Length; i++)
		{
			playerGameOver[i] = false;
		}
	}

	public IEnumerator MainCoroutine()
	{
		fadeController.isFadeIn = true;
		yield return StartCoroutine(GameStart());

		yield return StartCoroutine(GameMainLoop());

		yield return StartCoroutine(GameEnd());
	}

	private IEnumerator GameStart()
	{
		for (i = 3;i >= 1;i--)
		{
			uiManager.CountDown(i.ToString()); //カウントの文字を変更
			SoundManager.Instance.PlaySE("Count"); //カウントのSEを鳴らす
			yield return new WaitForSeconds(1.0f); //1秒待つ
		}
		uiManager.CountDown("すたーと");
		GameStatusManager.Instance.GameStart = false;
		yield return new WaitForSeconds(1.0f);
		uiManager.CountTextDelete();
	}

	private IEnumerator GameMainLoop()
	{
		bool flag = false;
		/*while (!GameStatusManager.Instance.GameEnd) {
		
			yield return new WaitForSeconds(0.1f);
			count++;
		}*/
		while (!flag)
		{
			int gameOverCount = 0;

			for (i = 0; i < playerGameOver.Length; i++)
			{
				if (playerGameOver[i])
				{
					gameOverCount++;
					if (photonManager.players - 1 <= gameOverCount)
						flag = true;
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public IEnumerator GameEnd()
	{
		SoundManager.Instance.PlaySE("GameEnd");

		uiManager.EndText();
		yield return new WaitForSeconds(1.0f);

		fadeController.isFadeOut = true;
		
		//SceneTransitionManager.Instance.SceneTransition();
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
