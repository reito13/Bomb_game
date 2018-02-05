using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour {

	//---------------------------------------------------------------------------------------------------------------------------------
    public string GetJson(SyncPlayerData data) //SyncPlayerクラスを渡すことで、同期させたい変数をJson形式にしてstring型で返す
    {
        return JsonUtility.ToJson(data);
    }

    public SyncPlayerData GetClass(string json) //Json形式のstringを渡すことで、Json形式からSyncPlayerクラスに戻して返す
    {
        return JsonUtility.FromJson<SyncPlayerData>(json);
    }

	public Vector3 GetPosition(string json)
	{
		SyncPlayerData asyncPlayer = JsonUtility.FromJson<SyncPlayerData>(json);
		return asyncPlayer.syncPos;
	}

	public float GetEularAngelY(string json)
	{
		SyncPlayerData asyncPlayer = JsonUtility.FromJson<SyncPlayerData>(json);
		return asyncPlayer.syncRoY;
	}
	//------------------------------------------------------------------------------------------------------------------------------------
	public string GetJson(SyncBombData data) //BombDataクラスを渡すことで、同期させたい変数をJson形式にしてstring型で返す
	{
		return JsonUtility.ToJson(data);
	}

	public SyncBombData GetBombData(string json) //Json形式のstringを渡すことで、Json形式からBombDataクラスに戻して返す
	{
		return JsonUtility.FromJson<SyncBombData>(json);
	}

	public Vector3[] GetBombPosition(string json)
	{
		SyncBombData syncBombData = JsonUtility.FromJson<SyncBombData>(json);
		return syncBombData.syncPos;
	}

}
