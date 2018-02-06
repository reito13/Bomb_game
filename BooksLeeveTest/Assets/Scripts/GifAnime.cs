using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifAnime : MonoBehaviour
{

    public Texture[] PlayerTexture;
    public float gifNum = 0;
    public float fps = 24;

    void Update()
    {
        gifNum = Time.time * fps;
        gifNum = gifNum % PlayerTexture.Length;
    //    renderer.material.mainTexture = PlayerTexture[(int)gifNum];
        Debug.Log(Mathf.Floor(gifNum));
    }
}
	
