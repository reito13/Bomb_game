using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URedis;
using System.Threading.Tasks;

public class TransformGetter : MonoBehaviour
{

    private float posX;
    private float posY;
    private float posZ;

    [SerializeField] private TransformSetter setter;

    private async void FixedUpdate()
    {
        posX = await setter.TransformGet("posX");
        posY = await setter.TransformGet("posY");
        posZ = await setter.TransformGet("posZ");

        transform.position = new Vector3(posX, posY, posZ);
    }
}
