using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class InputController : MonoBehaviour
{
	public async void Start()
	{
		await RedisSingleton.Instance.RedisSet("Jump1", "false");
		await RedisSingleton.Instance.RedisSet("Jump2", "false");
	}

	public async void MoveSet(float x, float y, int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string valueX = x.ToString();
		string valueY = y.ToString();
		await RedisSingleton.Instance.RedisSet("MoveInput" + "X" + number.ToString(), valueX);
		await RedisSingleton.Instance.RedisSet("MoveInput" + "Y" + number.ToString(), valueY);
	}

	public async Task<float> MoveGet(string dir, int number)
	{
		float x = await RedisSingleton.Instance.RedisGet("MoveInput" + dir + number.ToString(), false);
		return x;
	}

	/*public async void RotateSet(float x, float y, int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string valueX = x.ToString();
		string valueY = y.ToString();
		await RedisSingleton.Instance.RedisSet("RotateInput" + "X" + number.ToString(), valueX);
		await RedisSingleton.Instance.RedisSet("RotateInput" + "Y" + number.ToString(), valueY);
	}

	public async Task<float> RotateGet(string dir, int number)
	{
		float x = await RedisSingleton.Instance.RedisGet("RotateInput" + dir + number.ToString(), false);
		return x;
	}*/
	public async void RotateSet(float y,int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string value = y.ToString();
		await RedisSingleton.Instance.RedisSet("RotateInput" + number.ToString(), value);
	}

	public async Task<float> RotateGet(int number)
	{
		float value = await RedisSingleton.Instance.RedisGet("RotateInput" + number.ToString(), false);
		return value;
	}

	public async void JumpFlagSet(int number)
	{
		await RedisSingleton.Instance.RedisSet("Jump" + number.ToString(), "true");
	}
	public async Task<bool> JumpFlagGet(int number)
	{
		if (await RedisSingleton.Instance.RedisGet("Jump" + number.ToString()) == "true")
		{
			await RedisSingleton.Instance.RedisSet("Jump" + number.ToString(), "false");
			return true;
		}
		else
		{
			return false;
		}
	}
}
