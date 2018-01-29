using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTest : MonoBehaviour {

	// Use this for initialization
	private async void Start () {
		await RedisSingleton.Instance.RedisSet("a","b");
		Debug.Log(await RedisSingleton.Instance.RedisGet("a"));
	}
	
}
