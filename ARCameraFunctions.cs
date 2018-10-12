using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCameraFunctions : MonoBehaviour {

    public static ARCameraFunctions instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform GetCameraTransform()
    {
        return GetComponent<Transform>();
    }
}
