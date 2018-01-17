using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour {

	public Transform[] bombs = new Transform[6];

	private int i;
	private int mainNum;

	private void Awake()
	{
		mainNum = MainManager.playerNum - 1;	
	}
	private void FixedUpdate()
	{
		TransformSet();
		TransformGet();
	}

	private async void TransformSet()
	{
		for (i = mainNum * 3; i < 3 + (mainNum * 3); i++) //1Pのときはbombs[0~2]、2Pのときはbombs[3~5]に処理を行う
		{
			if(bombs[i] != null)
			{
				Debug.Log(i);
				await RedisSingleton.Instance.RedisSet("Bomb,X," + i.ToString(), bombs[i].position.x.ToString());
				await RedisSingleton.Instance.RedisSet("Bomb,Y," + i.ToString(), bombs[i].position.y.ToString());
				await RedisSingleton.Instance.RedisSet("Bomb,Z," + i.ToString(), bombs[i].position.z.ToString());
			}
		}
	}
	private async void TransformGet()
	{
		for (i = mainNum * 3; i < 3 + (mainNum * 3); i++) //1Pのときはbombs[0~2]、2Pのときはbombs[3~5]に処理を行う
		{
			if (bombs[i] != null)
			{
				Debug.Log(i);
				float x = await RedisSingleton.Instance.RedisGet("Bomb,X," + i.ToString(),false);
				float y = await RedisSingleton.Instance.RedisGet("Bomb,Y," + i.ToString(),false);
				float z = await RedisSingleton.Instance.RedisGet("Bomb,Z," + i.ToString(),false);

				Vector3 pos = new Vector3(x,y,z);
				if(pos == null)
				{
					Debug.Log("posがnullです");
				}
				if(bombs[i] == null)
				{
					Debug.Log("bombs[i].transform.positionがnullです");
				}
				bombs[i].transform.position = pos;
			}
		}
	}

}
