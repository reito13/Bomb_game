using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour {

	private MenuPlayer player;
	private CameraController cameraScript;
	private BombLanding bombLanding;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private float bombInterval = 0;
	private float bombIntervalMax = 1.0f;

	private void Start()
	{
		player = GetComponent<MenuPlayer>();
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
		}

		if (Input.GetMouseButtonDown(0))
		{
			StartCoroutine(player.ThrowBomb());
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
				int num;
				int rand = Random.Range(1, 11);

				switch (rand)
				{
					case 1:
						num = 2;
						break;
					case 2:
						num = 3;
						break;
					case 3:
						num = 4;
						break;
					case 4:
						num = 5;
						break;
					case 5:
						num = 6;
						break;

					default:
						num = 1;
						break;
				}
				StartCoroutine(player.SetBombType(num));
				Destroy(other.gameObject, 0.2f);
			}
		}
	}
}
