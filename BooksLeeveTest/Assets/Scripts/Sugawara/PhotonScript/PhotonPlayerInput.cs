using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerInput : MonoBehaviour {

	private PhotonPlayerController player = null;
	private CameraController cameraScript = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private void Start()
	{
		player = GetComponent<PhotonPlayerController>();
		cameraScript = GetComponent<CameraController>();
	}

	private void Update()
	{		
		if (!player.Control)
		{
			player.SetMoveDir(0, 0);
			return;
		}

		MoveInput();
		RotateInput();

		if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
		{
			player.Jump();
			//player.photonView.RPC("Jump",PhotonTargets.AllViaServer);
		}

		if ((Input.GetKeyDown(KeyCode.Z)) || (Input.GetButtonDown("R1") || Input.GetMouseButtonDown(0)))
		{
			bombSet = true;
		}
		if ((Input.GetKeyUp(KeyCode.Z)) || (Input.GetButtonUp("R1") || Input.GetMouseButtonUp(0)))
		{
			player.Bomb(bombTime);
			bombSet = false;
			bombTime = 0.0f;
		}

		if (bombSet)
			bombTime += Time.deltaTime;

	}

	private void MoveInput()
	{
		player.SetMoveDir(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void RotateInput()
	{
		cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
	}
}
