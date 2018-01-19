﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerTransformManager : MonoBehaviour {

	[SerializeField] private Transform[] playerTransforms = null; //全プレイヤーのTransform
	private Vector3 syncPos; //サーバーからGetした全プレイヤーの座標を格納する変数。座標移動の補間に使用する
	private float syncRotateY;

	private int i,j; //ループを回す用の変数
	private int transformCount; //playerTransformsの配列数
	private int myPlayerNum; //自分が操作するプレイヤーの番号 例:1なら1P

	[SerializeField] float lerpRate = 15; //2ベクトル間を補間する

	private void Start()
	{
		transformCount = playerTransforms.Length;
		myPlayerNum = MainManager.playerNum;
		myPlayerNum--;

		Task task = TransformCoroutine();
	}

	private void FixedUpdate()
	{
		for (j = 0; j < transformCount; j++)
		{
			if (myPlayerNum != j)
			{
				LerpPosition(j);
				LerpRotation(j);
			}
		}
		Debug.Log(playerTransforms[myPlayerNum].eulerAngles.y);

	}

	private async Task TransformCoroutine()
	{
		while (true) {
			await TransformSet();
			await TransformGet();
			await Task.Delay(100);
		}
	}

	private async Task TransformSet()
	{
		Task x = RedisSingleton.Instance.RedisSet("PlayerPositionX," + myPlayerNum.ToString(),(playerTransforms[myPlayerNum].position.x).ToString()); //プレイヤーのX座標をセット
		Task y = RedisSingleton.Instance.RedisSet("PlayerPositionY," + myPlayerNum.ToString(), (playerTransforms[myPlayerNum].position.y).ToString()); //プレイヤーのY座標をセット
		Task z = RedisSingleton.Instance.RedisSet("PlayerPositionZ," + myPlayerNum.ToString(), (playerTransforms[myPlayerNum].position.z).ToString()); //プレイヤーのZ座標をセット

		Task roY = RedisSingleton.Instance.RedisSet("PlayerRotationY," + myPlayerNum.ToString(), (playerTransforms[myPlayerNum].eulerAngles.y).ToString()); //プレイヤーのY角度をセット
		Debug.Log(roY);
		await Task.WhenAll(x,y,z,roY); //Task待機
	}

	private async Task TransformGet()
	{
		for (i = 0; i < transformCount; i++)
		{
			if (myPlayerNum != i)
			{
				float x = await RedisSingleton.Instance.RedisGet("PlayerPositionX," + i.ToString(), false);
				float y = await RedisSingleton.Instance.RedisGet("PlayerPositionY," + i.ToString(), false);
				float z = await RedisSingleton.Instance.RedisGet("PlayerPositionZ," + i.ToString(), false);

				float roY = await RedisSingleton.Instance.RedisGet("PlayerRotationY," + i.ToString(),false);

				syncPos = new Vector3(x, y, z);
				syncRotateY = roY;
			}
		}

	}

	private void LerpPosition(int num) //プレイヤーの移動を補間する処理。TransformGet()のfor文から呼び出され、numにはfor文のiが入る
	{
		playerTransforms[num].position = Vector3.Lerp(playerTransforms[num].position, syncPos, Time.deltaTime * lerpRate);
	}

	private void LerpRotation(int num)
	{
		Vector3 ro = playerTransforms[num].eulerAngles;
		//quaternion.y = Mathf.Lerp(quaternion.y,syncRotateY,Time.deltaTime * lerpRate);
		//quaternion.y = syncRotateY;
		//playerTransforms[num].rotation = Quaternion.Slerp(playerTransforms[num].rotation,quaternion,Time.deltaTime);
		ro.y = syncRotateY;//Mathf.Lerp(ro.y,syncRotateY,Time.deltaTime * lerpRate);
		playerTransforms[num].rotation = Quaternion.Euler(ro);
	}

}