using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncPlayer : BaseAsyncLoop {

    [SerializeField] JsonTest jsonScript = null; 

    [System.NonSerialized] public Transform playerTransform1; //1PのTransform
    [System.NonSerialized] public Transform playerTransform2; //2PのTransform


}
