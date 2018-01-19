using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BaseAsyncLoop : MonoBehaviour
{

	private void Start()
	{
		Task task = TransformCoroutine();
	}

	private async Task TransformCoroutine()
	{
		while (true)
		{
			await Set();
			await Get();
			await Task.Delay(100);
		}
	}

	protected async virtual Task Set()
	{
		await Task.Delay(100);
	}

	protected async virtual Task Get()
	{
		await Task.Delay(100);
	}
}