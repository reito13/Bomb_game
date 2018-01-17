using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class TimeManager : MonoBehaviour {

	private float timer;
	[SerializeField] private int timeCount = 60;


    private void Start()
	{
		timer = 0.0f;
	}

	private async void FixedUpdate()
	{
		if (!GameStatusManager.Instance.NormalState)
			return;

		if (MainManager.playerNum == 1)
		{
			TimeCount();
           await  TimeSet(timeCount, MainManager.playerNum);
        }
		else
		{
            timeCount = await TimeGet();
            MainManager.Instance.TimeUpdate(timeCount);
            if (timeCount <= 0)
            {
                StartCoroutine(MainManager.Instance.GameEnd());
            }
        }

	}


    private async Task <int> TimeGet()
	{
        float testTime = await RedisSingleton.Instance.RedisGet("TimeManager1",false);
        return (int)testTime;
    }
	private async Task TimeSet(int timeCount, int number)
	{
        string currentTime = timeCount.ToString();
        await RedisSingleton.Instance.RedisSet("TimeManager1", currentTime);
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
