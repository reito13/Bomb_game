using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private Player player = null;
	[SerializeField] private CameraController cameraScript = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private MoveInputSetter moveInputSetter;

	private void Awake()
	{
		moveInputSetter = GetComponent<MoveInputSetter>();
	}

	private void Update()
	{
		if (!player.Control)
		{
			player.SetMoveDir(0,0);
			return;
		}
		if (player.number == MainManager.playerNum) //このスクリプトをアタッチしたプレイヤーキャラの番号が操作キャラの番号であるとき
		{
			MoveInput();
			RotateInput();

			if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
			{
				player.Jump();
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
		}
		
		if (bombSet)
			bombTime += Time.deltaTime;

		MoveInputGet();
	}

	private void MoveInput()
	{
		if (player.number == MainManager.playerNum)
		{
			moveInputSetter.InputSet(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), player.number);
		}
	}

	private async void MoveInputGet()
	{
		player.SetMoveDir(await moveInputSetter.InputGet("X", player.number), await moveInputSetter.InputGet("Y", player.number));
	}

	private async void RotateInput()
	{
		if(player.number == MainManager.playerNum)
		{
			//moveInputSetter.RotateSet();
		}
		cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
	}
}
