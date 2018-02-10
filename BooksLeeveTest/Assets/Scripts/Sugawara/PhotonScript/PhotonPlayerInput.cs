using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerInput : MonoBehaviour {

	private PhotonPlayerController player;
	private CameraController cameraScript;
	private BombLanding bombLanding;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private float bombInterval = 0;
	private float bombIntervalMax = 1.0f;

	private void Start()
	{
		player = GetComponent<PhotonPlayerController>();
		cameraScript = GetComponent<CameraController>();
		bombLanding = GetComponent<BombLanding>();
	}

	private void Update()
	{
		RotateInput();

		if (!player.Control || player.damaged || player.picked || player.throwed)
		{
			player.SetMoveDir(0, 0);
			return;
		}

		MoveInput();
		BombLandPosInput();
		ActionInput();

		if (bombSet)
			bombTime += Time.deltaTime;

		bombInterval += Time.deltaTime;
	}

	private void ActionInput()
	{
		if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
		{
			player.Jump();
			//player.photonView.RPC("Jump",PhotonTargets.AllViaServer);
		}

		if ((Input.GetKeyDown(KeyCode.Z)) || (Input.GetButtonDown("R1") || Input.GetMouseButtonDown(0)))
		{
			//bombSet = true;
		}
		if ((Input.GetKeyUp(KeyCode.Z)) || (Input.GetButtonUp("R1") || Input.GetMouseButtonUp(0)))
		{
			if (bombInterval < bombIntervalMax)
				return;

			bombInterval = 0;
			player.StartCoroutine(player.Bomb(bombTime));
			bombSet = false;
			bombTime = 0.0f;
		}

		if (Input.GetMouseButtonDown(1))
		{
			StartCoroutine(player.SPBomb(0));
		}
	}

	private void MoveInput()
	{
		player.SetMoveDir(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void RotateInput()
	{
		cameraScript.SetRotate(Input.GetAxis("Mouse X"));
	}

	private void BombLandPosInput()
	{
		bombLanding.SetBombLandingPosition(Input.GetAxis("Mouse Y"));
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Carrot")
		{
			if (Input.GetMouseButtonDown(1))
			{
				player.SetBomb();
				Destroy(other.gameObject);
			}
		}
	}
}
