using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using URedis;

public class Sample : MonoBehaviour {

    [SerializeField] private string ipAddress;
    [SerializeField] private int port = 6379;
    [SerializeField] private string key="test";
    [SerializeField] private string value = "hoge";

    private Redis redis;

    private void Start() {
        ExampleConnect();
    }

    private void Update() {
        // 左クリックでセット
        if(Input.GetMouseButtonDown(0)) {
            ExampleSet();

        // 右クリックでゲット
        } else if(Input.GetMouseButtonDown(1)) {
            ExampleGet();
        }
    }

    private async void ExampleSet() {
        await redis.Set(key, value);
        Debug.Log("Setted");
    }

    private async void ExampleGet() {
        Task<string> getter = redis.Get(key);
        string data = await getter;
        Debug.Log(data);
    }

    public async void ExampleConnect() {
        redis = new Redis();
        await redis.Connect(ipAddress, port);
        Debug.Log("Connected");
    }
}
