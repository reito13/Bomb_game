using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private Player player = null;

	private void Update()
	{
		player.SetMoveDir(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
		player.SetRotate(Input.GetAxis("RotateVertical"),Input.GetAxis("RotateHorizontal"));

		if (Input.GetKeyDown(KeyCode.Z))
		{
			player.Bomb();
		}
	}
}
