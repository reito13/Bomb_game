using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	private float yRotate;

	[SerializeField] private float mouseSpeed = 0.0f;
	[SerializeField] private float cameraSpeed = 0.0f;

	//[SerializeField] private float roteteLimitX = 0.0f;

	private Quaternion charaRotate;
	[SerializeField] private Transform charaTransform = null;
	private Quaternion cameraRotate;
	[SerializeField] private Transform cameraTransform = null;

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		charaRotate = charaTransform.localRotation;
		cameraRotate = cameraTransform.localRotation;
	}
	public void SetRotate(float y)
	{
		yRotate = y;
	}

	public void RotatitonUpdate()
	{
		yRotate *= mouseSpeed;
		charaRotate *= Quaternion.Euler(0f, yRotate, 0f);
		charaTransform.localRotation = Quaternion.Slerp(charaTransform.localRotation, charaRotate, cameraSpeed * Time.deltaTime);
	}
}
