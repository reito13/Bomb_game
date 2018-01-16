using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class TransformSetter : MonoBehaviour {


	[System.NonSerialized] public Player player;

    public bool set = false;

	private void Awake()
	{
		player = GetComponent<Player>();
	}

    private void Start()

    {
        if (player.number != MainManager.playerNum) //プレイヤーの番号が操作キャラの番号と同じとき
        {
            this.enabled = false;
        }
    }

    private async void FixedUpdate()
    {
        await TransformSet("pos",transform.position);
    }

    private async Task TransformSet(string key,Vector3 pos)
    {
        string valueX = (pos.x).ToString();
        string valueY = (pos.y).ToString();
        string valueZ = (pos.z).ToString();
		// await redis.Set(key + "X" + player.number, valueX);
		// await redis.Set(key + "Y" + player.number, valueY);
		// await redis.Set(key + "Z" + player.number, valueZ);
		await RedisSingleton.Instance.RedisSet(key + "X" + player.number, valueX);
		await RedisSingleton.Instance.RedisSet(key + "Y" + player.number, valueY);
		await RedisSingleton.Instance.RedisSet(key + "Z" + player.number, valueZ);

	}

	public async Task<float> TransformGet(string key)
    {
		return await RedisSingleton.Instance.RedisGet(key,false);
    }

}
