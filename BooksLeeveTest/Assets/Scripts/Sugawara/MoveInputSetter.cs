using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class MoveInputSetter : MonoBehaviour {

	public async void InputSet(float x, float y, int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string valueX = x.ToString();
		string valueY = y.ToString();
		await RedisSingleton.Instance.RedisSet("MoveInput" + "X" + number.ToString(), valueX);
		await RedisSingleton.Instance.RedisSet("MoveInput" + "Y" + number.ToString(), valueY);
	}

	public async Task<float> InputGet(string dir,int number)
	{
		float x =  await RedisSingleton.Instance.RedisGet("MoveInput" + dir + number.ToString(),false);
		return x;
	}

	public async void RotateSet(float x, float y, int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string valueX = x.ToString();
		string valueY = y.ToString();
		await RedisSingleton.Instance.RedisSet("MoveInput" + "X" + number.ToString(), valueX);
		await RedisSingleton.Instance.RedisSet("MoveInput" + "Y" + number.ToString(), valueY);
	}

	public async Task<float> RotateGet(string dir, int number)
	{
		float x = await RedisSingleton.Instance.RedisGet("MoveInput" + dir + number.ToString(), false);
		return x;
	}
}
