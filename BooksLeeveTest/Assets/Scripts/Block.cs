using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] Collider collider = null;
	[SerializeField] MeshRenderer renderer = null;

	private void OnBecameVisible()
	{
		collider.enabled = true;
		renderer.enabled = true;
	}

	private void OnBecameInvisible()
	{
		//collider.enabled = false;
		renderer.enabled = false;
	}
}
