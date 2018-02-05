using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLanding : MonoBehaviour {

	public Transform bombLandTransform = null;

	[SerializeField] private float speed = 10.0f;

	private float setY;

	//6.5:240 ~ 25.5:515 = 19:275
	//6:250 ~ 36:720

	private void FixedUpdate()
	{
		Move();
	}

	private void Move() {
		bombLandTransform.position = bombLandTransform.position + (transform.forward * setY * speed);
	}

	public void SetBombLandingPosition(float y)
	{
		setY = y;
	}

	public float GetPower(Transform playerTransform)
	{
		float distance = Vector3.Distance(bombLandTransform.position, playerTransform.position);
		Debug.Log(distance);
		float baseDistance = distance - 6.0f;
		float power = 250 + (baseDistance * 470/30);
		power = power + Mathf.Abs(15 - Mathf.Abs(baseDistance - 15)) * 5f;
		return power;
		//return Vector3.Distance(bombLandTransform.position, playerTransform.position);
	}


}
