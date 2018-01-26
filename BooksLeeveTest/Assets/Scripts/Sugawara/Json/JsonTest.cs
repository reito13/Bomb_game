using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonTest : MonoBehaviour {

    public string GetJson(AsyncPlayer player)
    {
        return JsonUtility.ToJson(player);
    }

    public AsyncPlayer GetClass(string json)
    {
        return JsonUtility.FromJson<AsyncPlayer>(json);
    }

}
