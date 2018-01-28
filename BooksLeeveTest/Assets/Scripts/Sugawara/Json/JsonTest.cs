using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour {

    public string GetJson(AsyncPlayerData playerData) //AsyncPlayerクラスを渡すことで、同期させたい変数をJson形式にしてstring型で返す
    {
        return JsonUtility.ToJson(playerData);
    }

    public AsyncPlayerData GetClass(string json) //Json形式のstringを渡すことで、Json形式からAsyncPlayerクラスに戻して返す
    {
        return JsonUtility.FromJson<AsyncPlayerData>(json);
    }

	public Vector3 GetPosition(string json)
	{
		AsyncPlayerData asyncPlayer = JsonUtility.FromJson<AsyncPlayerData>(json);
		return asyncPlayer.syncPos;
	}

	public float GetEularAngelY(string json)
	{
		AsyncPlayerData asyncPlayer = JsonUtility.FromJson<AsyncPlayerData>(json);
		return asyncPlayer.syncRoY;
	}

}
