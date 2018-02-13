using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingArea : MonoBehaviour {

	private Transform myTransform;
	private Vector3 myPos;
	[SerializeField] private Transform target = null;

	[SerializeField] private Transform blockCheck = null;

	[SerializeField] private float baseYPos = 0;

	private Ray ray = new Ray();
	private RaycastHit hit;

	private int i;
	private Vector3 checkPos;
	private float saveY;

	[SerializeField] private Material material;

	[SerializeField] private Color[] colors = new Color[2];

	private void Awake()
	{
		myTransform = GetComponent<Transform>();
		//material = GetComponent<Renderer>().material;
		colors[0] = material.color;
	}
	private void Update()
	{
		myPos = target.position;
		for (i = 6; i > -2; i--)
		{
			if (HeightCheck(i))
			{
				saveY = baseYPos + i + 1;
				break;
			}
		}
		myPos.y = saveY;
		//myPos.y = baseYPos + 
		myTransform.position = myPos;
	}

	private bool HeightCheck(int count)
	{
		checkPos = blockCheck.position;
		checkPos.y = baseYPos + count;
		ray.origin = checkPos;
		ray.direction = transform.right * -1;

		if (Physics.Raycast(ray, out hit, 0.1f))
		{
			if (hit.collider.tag == "Block")
				return true;
			else
				return false;
		}
		else
		{
			return false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			material.color = colors[1];
			material.SetColor("_EmissionColor", colors[1]);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			material.color = colors[0];
			material.SetColor("_EmissionColor", colors[0]);

		}
	}
}
