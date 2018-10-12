using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlanetRange : MonoBehaviour
{
    public ShowWorld world;
    public bool showInfo;
    public bool VRViewable;
    [HideInInspector]
    public Transform cameraPos;
    public GameObject planetInfo;
    public float radius;

    private ViewPlanet viewPlanet;
    private bool triggered = false;
    private float dist;

    void Start()
    {
        viewPlanet = ViewPlanet.instance;
        cameraPos = ARCameraFunctions.instance.GetCameraTransform();
    }

    void Update()
    {        
        dist = Vector3.Distance(cameraPos.position, transform.position);        
        if (dist < radius && !triggered)
        {
            triggered = true;
            if (viewPlanet.cameraState == CameraState.AR)
            {
                if (VRViewable)
                {
                    viewPlanet.SetPlanet(world);
                }                
            }                        
            if (showInfo)
            {
                planetInfo.SetActive(true);
            }
        }
        if (dist > radius && triggered)
        {
            triggered = false;
            if (showInfo)
            {
                planetInfo.SetActive(false);
            }
            if (VRViewable)
            {
                viewPlanet.ResetPlanet();
            }
        }        
    }
}