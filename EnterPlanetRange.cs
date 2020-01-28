using UnityEngine;

public class EnterPlanetRange : MonoBehaviour
{
    public Location world;
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
        viewPlanet = ViewPlanet.Instance;
        cameraPos = ARCamera.Instance.CameraTransform;
    }

    void Update()
    {        
        dist = Vector3.Distance(cameraPos.position, transform.position);        
        if (dist < radius * ViewPlanet.Instance.SolarSystemScale && !triggered)
        {
            triggered = true;
            if (ARCamera.Instance.cameraState == CameraState.AR)
            {
                if (VRViewable)
                    viewPlanet.SetPlanet(world);
            }                        
            if (showInfo)
                planetInfo.SetActive(true);
        }
        if (dist > radius * ViewPlanet.Instance.SolarSystemScale && triggered)
        {
            triggered = false;
            if (showInfo)
                planetInfo.SetActive(false);
            if (VRViewable)
                ViewPlanet.Instance.LandingCanvas.SetActive(false);
        }        
    }
}