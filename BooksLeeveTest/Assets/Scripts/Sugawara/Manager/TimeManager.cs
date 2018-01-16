using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeManager : MonoBehaviour {

	private float timer;
	[SerializeField] private int timeCount = 60;


    private void Start()
	{
		timer = 0.0f;
	}

	private void Update()
	{
		if (!GameStatusManager.Instance.NormalState)
			return;

		if (MainManager.playerNum == 1)
		{
			TimeCount();
			TimeSet();
		}
		else
		{
			TimeGet();
		}

	}

	private void TimeGet()
	{
      //  timeCount
	}
	private void TimeSet()
	{
       // timeCount
	}

	private void TimeCount()
	{
		timer += Time.deltaTime;
		if (timer >= 1.0f)
		{
			timer = 0.0f;
			timeCount--;
			MainManager.Instance.TimeUpdate(timeCount);
			if (timeCount <= 0)
			{
				StartCoroutine(MainManager.Instance.GameEnd());
			}
		}
	}
}
