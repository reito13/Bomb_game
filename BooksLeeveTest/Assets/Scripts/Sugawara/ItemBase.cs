using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

	[SerializeField] private PhotonPlayerController.BombType bombType = PhotonPlayerController.BombType.NONE;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			other.GetComponent<PhotonPlayerController>().SetBombType(bombType);
		}
	}
}
