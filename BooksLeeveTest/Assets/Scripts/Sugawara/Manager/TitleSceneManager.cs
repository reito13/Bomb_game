using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {

	[SerializeField] private SceneTransitionManager sceneManager = null;
	
	void Update () {
		if (Input.anyKeyDown)
		{
			sceneManager.SceneTransition();
		}
	}
}
