using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;



public class BombGetter : MonoBehaviour {

    public Player player;

    private float posX;
    private float posY;
    private float posZ;

    [SerializeField] private BombSetter setter;

    private void Awake()
    {
        Debug.Log(player.number);
        Debug.Log(MainManager.playerNum);
        if (player.number == MainManager.playerNum)
        {
            this.enabled = false;
        }
    }

    private async void FixedUpdate()
    {

        posX = await setter.TransformGet("posX" + player.number);
        posY = await setter.TransformGet("posY" + player.number);
        posZ = await setter.TransformGet("posZ" + player.number);

        transform.position = new Vector3(posX, posY, posZ);
    }
}
