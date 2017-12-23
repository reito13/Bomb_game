using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Redis;

public class UredisTest : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {
        Trigger();
    }

    async void Trigger ()
    {
     /*   Redis.Redis redis = new Redis.Redis();


        if (await (bool)redis.Connect("133.18.173.94").Result)
        {
            Debug.Log("接続成功");
        }

        else
        {
            Debug.LogError("接続失敗");
        }
        */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
