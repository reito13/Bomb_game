using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

	public Transform rayPosition;
	public float rayRange;
	private Ray ray = new Ray();
	private RaycastHit hit;
	//private RaycastHit hit;

	public bool Grounded()
	{
		ray.origin = rayPosition.position;
		ray.direction = transform.up * -1;

		if (Physics.Raycast(ray, out hit, 0.1f))
		{
			if (hit.collider.tag == "Block")
			{
				return true;
			}
			else
			{
				return true;
			}
		}
		else
		{
			return false;
		}
	}
}
