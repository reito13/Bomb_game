﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BaseAsyncLoop : MonoBehaviour
{
	public RedisSingleton redis;

	private float timer = 0;
	private bool timerFlag = false;
    public int count = 0;

	protected virtual void Awake()
	{
		count = 0;
		Task task = TransformCoroutine();
	}

	protected virtual async Task TransformCoroutine()
	{
		while (true)
		{
			if (redis.connect)
   			{
                count++;
				await Set(count);
                await Task.Delay(100);
                await Get(count);
			}
		}
	}

	protected async virtual Task Set(int count)
	{
		await Task.Delay(0);
	}

	protected async virtual Task Get(int count)
	{
		await Task.Delay(0);
	}

	private void Update()
	{
		if (timerFlag)
			timer += Time.deltaTime;
	}

	protected void CountStart()
	{
		timerFlag = true;
		timer = 0.0f;
	}

	protected void CountEnd()
	{
		Debug.Log(timer);
		timerFlag = false;
		timer = 0;
	}
}