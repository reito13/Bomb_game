using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

	public Transform rayPosition;
	public float rayRange;
	private Ray ray = new Ray();
	//private RaycastHit hit;

	public bool Grounded()
	{
		ray.origin = rayPosition.position;
		ray.direction = transform.up * -1;
		if (Physics.Raycast(ray, rayRange))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
