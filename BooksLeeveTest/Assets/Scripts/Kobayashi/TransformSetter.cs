using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class TransformSetter : MonoBehaviour {


    [SerializeField] private string ipAddress;
    [SerializeField] private int port = 6379;

    private Redis redis;

    private void Start()
    {
        ExampleConnect();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        TransformSet("pos",transform.position);
        TransformGet("posX");
        TransformGet("posY");
        TransformGet("posZ");
    }

    private async void TransformSet(string key,Vector3 pos)
    {
        string valueX = (pos.x).ToString();
        string valueY = (pos.y).ToString();
        string valueZ = (pos.z).ToString();
        await redis.Set(key + "X", valueX);
        await redis.Set(key + "Y", valueY);
        await redis.Set(key + "Z", valueZ);

    }

    public async Task<float> TransformGet(string key)
    {
        Task<string> getter = redis.Get(key);
        string data = await getter;
        Debug.Log(data);
        return float.Parse(data);
    }

    /*private async void ExampleSet()
    {

        Debug.Log("Setted");
    }

   

    private async void ExampleGet()
    {
        Task<string> getter = redis.Get(key);
        string data = await getter;
        Debug.Log(data);
    }*/

    public async void ExampleConnect()
    {
        redis = new Redis();
        await redis.Connect(ipAddress, port);
        Debug.Log("Connected");
    }
}
