using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class MoveInputSetter : MonoBehaviour {

	private string ipAddress = "133.18.173.94";
	private int port = 6379;
	private Redis redis;

	public async void InputSet(float x, float y, int number) //Horizontal,Verticalの入力、プレイヤー番号を渡す
	{
		string valueX = x.ToString();
		string valueY = y.ToString();
		await redis.Set("MoveInput" + "X" + number.ToString(), valueX);
		await redis.Set("MoveInput" + "Y" + number.ToString(), valueY);
	}

	public async void ExampleConnect()
	{
		redis = new Redis();
		await redis.Connect(ipAddress, port);
		Debug.Log("Connected");
	}

}
