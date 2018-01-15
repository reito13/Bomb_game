using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class TransformGetter : MonoBehaviour
{
    public Player player;

    private float posX;
    private float posY;
    private float posZ;

    [SerializeField] private TransformSetter setter;

    private void Start()
    {
        if (player.number == MainManager.playerNum)
        {
            this.enabled = false;
        }
    }

    private async void FixedUpdate()
    {
        if (setter.set)
        {
            posX = await setter.TransformGet("posX" + player.number);
            posY = await setter.TransformGet("posY" + player.number);
            posZ = await setter.TransformGet("posZ" + player.number);

            transform.position = new Vector3(posX, posY, posZ);
        }
    }
}
