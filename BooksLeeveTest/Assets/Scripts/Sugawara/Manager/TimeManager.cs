using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class TimeManager : MonoBehaviour {

	private float timer;
	public int timeCount = 60;

	public int id = -1;

    private void Start()
	{
		timer = 0.0f;
	}

	/*private async void Update()  //redis用
	{
		if (!GameStatusManager.Instance.NormalState)
			return;

		if (MainManager.playerNum == 1)
		{
			TimeCount();
        //   await  TimeSet(timeCount, MainManager.playerNum);
        }
		else
		{
           // timeCount = await TimeGet();

		}

	}*/

	private void Update()
	{
		if (!GameStatusManager.Instance.NormalState)
			return;
		if (id == 1)
		{
			TimeCount();
		}

		if (timeCount <= 0)
		{
			StartCoroutine(MainManager.Instance.GameEnd());
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
		}
	}
}
