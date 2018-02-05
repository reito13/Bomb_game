using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonData : MonoBehaviour {

}

public class BaseSyncData
{

}

public class SyncPlayerData : BaseSyncData
{
	public Vector3 syncPos;
	public float syncRoY;
}

public class SyncBombData : BaseSyncData
{
	public Vector3[] syncPos = new Vector3[6];
	public Vector3[] syncRo = new Vector3[6];
	public bool[] activeFlag = new bool[6];
	public bool[] explosionFlag = new bool[6];
}
