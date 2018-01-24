using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BaseAsyncLoop : MonoBehaviour
{

	private float timer = 0;
	private bool timerFlag = false;

	protected virtual void Awake()
	{
		Task task = TransformCoroutine();
	}

	protected virtual async Task TransformCoroutine()
	{
		while (true)
		{
			if (RedisSingleton.Instance.connect)
			{
				await Set();
				await Get();
			}
			await Task.Delay(100);
		}
	}

	protected async virtual Task Set()
	{
		await Task.Delay(0);
	}

	protected async virtual Task Get()
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