using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

	public enum AnimationStatus
	{
		WAIT,WALK,RUN
	}

	public AnimationStatus[] animStatus = new AnimationStatus[4];
	[SerializeField] Animator[] animators = new Animator[4];



	public void AnimationSet(int charaNum,AnimationStatus anim)
	{
		animStatus[charaNum] = anim;
	}

	private void AnimationUpdate()
	{

	}
}
