using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTest2 : Photon.MonoBehaviour
{

	private void Start()
	{
		Debug.Log(PhotonNetwork.room.Name);
	}
}
