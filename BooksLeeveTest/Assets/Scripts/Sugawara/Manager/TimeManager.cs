﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

	private float timer;
	private int timeCount;


	private void Start()
	{
		timer = 0.0f;
		timeCount = 60;
	}

	private void Update()
	{
		if (!GameStatusManager.Instance.NormalState)
			return;

		timer += Time.deltaTime;
		if(timer >= 1.0f)
		{
			timer = 0.0f;
			timeCount--;
			MainManager.Instance.TimeUpdate(timeCount);
			if (timeCount <= 0)
			{
				MainManager.Instance.GameEnd();
			}
		}
	}

}
