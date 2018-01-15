using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class TransformSetter : MonoBehaviour {

<<<<<<< HEAD
	public Player player;
=======
>>>>>>> parent of e3b73d9... プレイヤーの番号を選択できるように変更

    [SerializeField] private string ipAddress;
    [SerializeField] private int port = 6379;

    private Redis redis;

<<<<<<< HEAD
    public bool set = false;

	private void Awake()
	{
        ExampleConnect();

	}

	private void Start()
=======
    private void Start()
>>>>>>> e3db3e118b2c30e901d0212389132783c27db492
    {
        if (player.number != MainManager.playerNum)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        TransformSet("pos",transform.position);
    }

    private async void TransformSet(string key,Vector3 pos)
    {
        string valueX = (pos.x).ToString();
        string valueY = (pos.y).ToString();
        string valueZ = (pos.z).ToString();
        await redis.Set(key + "X" + player.number, valueX);
        await redis.Set(key + "Y" + player.number, valueY);
        await redis.Set(key + "Z" + player.number, valueZ);

    }

    public async Task<float> TransformGet(string key)
    {
        if (redis == null)
            Debug.Log("RedisError");
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
        set = true;
    }
}
