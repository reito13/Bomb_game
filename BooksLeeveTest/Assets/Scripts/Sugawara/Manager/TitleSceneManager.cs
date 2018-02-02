using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {

	[SerializeField] private SceneTransitionManager sceneManager = null;

	[SerializeField] private GameObject bombPrefab = null;
	[SerializeField] private Transform bombPos = null;
	[SerializeField] private Transform bombRo = null;

	void Update () {
		if (Input.anyKeyDown)
		{
			//sceneManager.SceneTransition();
			StartCoroutine(BombThrow());
		}
	}

	private IEnumerator BombThrow()
	{
		SoundManager.Instance.PlaySE("BombThrow");

		Quaternion bombRo = bombPos.rotation;
		bombRo.eulerAngles = bombRo.eulerAngles;

		GameObject bomb = Instantiate(bombPrefab) as GameObject;
		bomb.GetComponent<MenuBomb>().Set(bombPos.position, transform.rotation, 1.5f);

		yield return new WaitForSeconds(2.0f);
		sceneManager.SceneTransition();
	}
}
