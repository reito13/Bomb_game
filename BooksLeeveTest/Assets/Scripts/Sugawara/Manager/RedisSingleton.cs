// 作成者:菅原
// 機能:Redisを使ったデータセット、データゲットのシングルトン
// 作成日:2018_01_16

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class RedisSingleton : SingletonMonoBehaviour<RedisSingleton>
{

	[SerializeField] private string ipAddress = "133.18.173.94";
	[SerializeField] private int port = 6379;

	private Redis redis;

	private bool connect = false;

	private void Awake()
	{
		RedisSingleton.Instance.ExampleConnect();
	}

	public async Task RedisSet(string key,string value)
	{
		if (!connect)
			return;

		await redis.Set(key,value);
	}

	public async Task<string> RedisGet(string key)
	{
		Task<string> getter = redis.Get(key);
		string data = await getter;
		return data;
	}

	/// <summary>
	/// Redisサーバからデータを取得する。
	/// 第2引数にtrueを入れるとint型、falseを入れるとfloat型で返す
	/// </summary>
	/// <param name="key"></param>
	/// <param name="b"></param>
	/// <returns></returns>
	/// 
	public async Task<float> RedisGet(string key,bool b) //bool b が「true」だと「int型」、「false」だと「float型」が返る
	{
		if (!connect)
			return 0;

		Task<string> getter = redis.Get(key);
		string data = await getter;

		if (b)
		{
			return int.Parse(data);
		}
		else
		{
			return float.Parse(data);
		}
	}

	public async void ExampleConnect()
	{
		redis = new Redis();
		await redis.Connect(ipAddress, port);
		Debug.Log("Connected");
		connect = true;

	}
}
