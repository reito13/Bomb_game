using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingObject : MonoBehaviour {

	[SerializeField] private MenuPhotonManager menuPhotonManager = null;
	[SerializeField] private MenuPlayer menuPlayer = null;

	[SerializeField] private int players; //何人プレイか

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "MenuPlayer")
		{
			StartCoroutine(MatchingStart(other));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "MenuPlayer")
		{
			StartCoroutine(MatchingEnd(other));
		}
	}

	private IEnumerator MatchingStart(Collider other)
	{
		menuPhotonManager.Join(players,other.gameObject);
		menuPlayer.Control = false;
		yield return new WaitForSeconds(0.5f);
		menuPlayer.Control = true;

	}

	private IEnumerator MatchingEnd(Collider other)
	{
		menuPhotonManager.Leave();
		menuPlayer.Control = false;
		yield return new WaitForSeconds(0.5f);
		menuPlayer.Control = true;

	}
}
