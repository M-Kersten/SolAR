using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

[System.Serializable]
public class World
{
    public string name;
    public GameObject world;
    public Material sky;
    public ShowWorld showWorld;
    public GameObject info;
    public Material planetMat;
}

[System.Serializable]
public enum CameraState
{
    AR,
    VR,
    none
}

[System.Serializable]
public enum ShowWorld
{
    sun,
    mercury,
    venus,
    mars,
    earth,
    moon,    
    jupiter,
    saturn,
    uranus,
    neptune,
    ISS,
    none
}

public class ViewPlanet : MonoBehaviour {

    public CameraState cameraState;
    public Material milkyWayBackground;
    public World[] worlds;

    public static ViewPlanet instance;
    
    public ARCoreBackgroundRenderer BGRenderer;
    public GameObject solarSystem;
    public GameObject worldsGO;
    public GameObject buyFullUI;
    public GameObject buyFullUIinfo;

    public ShowWorld currentWorld = ShowWorld.none;
    public ShowWorld currentInfoWorld = ShowWorld.none;

    [Header("UI")]
    public Camera mainCamera;
    public Camera infoCamera;
    public GameObject infoCanvas;
    public GameObject[] planetsInfo;
    public GameObject infoPlanet;
    public GameObject ISS;
    public GameObject planetMap;
    public Text planetName;
    public Text planetNameVR;
    public GameObject LandingCanvas;
    public GameObject LeavingCanvas;
    public GameObject headerAR;
    public GameObject ARMain;
    public GameObject trackingLost;

    public GameObject[] layerCanvas;

    [HideInInspector]
    public FadeInObject solarSytemFadeIn;

    private void Awake()
    {
        RenderSettings.skybox = milkyWayBackground;
        instance = this;
        cameraState = CameraState.AR;
        Debug.Log("length of worlds: " + worlds.Length);
    }

    public void DisableText()
    {
        LandingCanvas.SetActive(false);
    }

    public void SetPlanet(ShowWorld world)
    {        
        currentWorld = world;
        planetName.text = GetWorld(world).name;
        planetNameVR.text = GetWorld(world).name;
        LandingCanvas.SetActive(true);               
    }

    public void ResetPlanet()
    {
        if (cameraState == CameraState.AR)
        {
            currentWorld = ShowWorld.none;
        }        
        LandingCanvas.SetActive(false);
    }

    public void ToVRScene()
    {
        if (IsFreePlanet(currentWorld))
        {
            foreach (World item in worlds)
            {
                item.world.SetActive(false);
            }
            worldsGO.transform.position = mainCamera.gameObject.transform.position;
            cameraState = CameraState.VR;
            LandingCanvas.SetActive(false);
            LeavingCanvas.SetActive(true);
            solarSystem.SetActive(false);
            headerAR.SetActive(false);
            if (infoCanvas.activeInHierarchy)
            {
                GetWorld(currentInfoWorld).world.SetActive(true);
                Debug.Log("name of currentinfo is: " + GetWorld(currentInfoWorld).name);
                planetNameVR.text = GetWorld(currentInfoWorld).name;
                RenderSettings.skybox = GetWorld(currentInfoWorld).sky;
            }
            else
            {
                GetWorld(currentWorld).world.SetActive(true);
                RenderSettings.skybox = GetWorld(currentWorld).sky;
            }
            BGRenderer.enabled = false;
            SetLayer(0);
        }
        else
        {
            buyFullUI.SetActive(true);
        }
    }
    public void ToAR()
    {
        cameraState = CameraState.AR;
        RenderSettings.skybox = milkyWayBackground;
        ResetPlanet();
        LandingCanvas.SetActive(false);
        LeavingCanvas.SetActive(false);
        solarSystem.SetActive(true);
        headerAR.SetActive(true);
        foreach (World item in worlds)
        {
            item.world.SetActive(false);
        }
        BGRenderer.enabled = true;
    }   
    
    public World GetWorld(ShowWorld showWorld)
    {
        int i;
        switch (showWorld)
        {
            case ShowWorld.earth:
                i = 0;
                break;
            case ShowWorld.moon:
                i = 1;
                break;
            case ShowWorld.mars:
                i = 2;
                break;
            case ShowWorld.venus:
                i = 3;
                break;
            case ShowWorld.mercury:
                i = 4;
                break;
            case ShowWorld.jupiter:
                i = 5;
                break;
            case ShowWorld.saturn:
                i = 6;
                break;
            case ShowWorld.uranus:
                i = 7;
                break;
            case ShowWorld.neptune:
                i = 8;
                break;
            case ShowWorld.ISS:
                i = 9;
                break;
            case ShowWorld.none:
                Debug.Log("no world selected");
                i = 0;
                break;
            default:
                Debug.Log("world not found");
                i = 0;
                break;
        }
        if (i < worlds.Length)
        {
            return worlds[i];
        }
        else
        {
            return worlds[0];
        }
    }

    public void ChangeBackground()
    {
        BGRenderer.enabled = !BGRenderer.enabled;
    }

    public void SetLayer(int layer)
    {
        solarSytemFadeIn.SetLayer(layer);
        foreach (GameObject item in layerCanvas)
        {
            item.SetActive(false);
        }
        layerCanvas[layer].SetActive(true);
    }

    public void ViewPlanetInfo(bool on)
    {
        currentInfoWorld = currentWorld;
       
        mainCamera.enabled = !on;
        infoCamera.enabled = on;
        headerAR.SetActive(!on);
        ARMain.SetActive(!on);
        infoCanvas.SetActive(on);
        if (on)
        {
            if (currentInfoWorld == ShowWorld.ISS)
            {
                ISS.SetActive(true);
                infoPlanet.SetActive(false);
            }
            else
            {
                ISS.SetActive(false);
                infoPlanet.SetActive(true);
                infoPlanet.GetComponent<Renderer>().material = GetWorld(currentInfoWorld).planetMat;
            }
            foreach (GameObject item in planetsInfo)
            {
                item.SetActive(false);
            }
            GetWorld(currentInfoWorld).info.SetActive(true);
        }
        if (!on)
        {
            if (cameraState == CameraState.AR)
            {
                headerAR.SetActive(true);
            }
            else
            {
                headerAR.SetActive(false);
            }
        }        
    }

    public void ViewPlanetInfo(int planet)
    {
        currentInfoWorld = worlds[planet].showWorld;

        mainCamera.enabled = false;
        infoCamera.enabled = true;
        headerAR.SetActive(false);
        ARMain.SetActive(false);
        infoCanvas.SetActive(true);
        if (currentInfoWorld == ShowWorld.ISS)
        {
            ISS.SetActive(true);
            infoPlanet.SetActive(false);
        }
        else
        {
            ISS.SetActive(false);
            infoPlanet.SetActive(true);
            infoPlanet.GetComponent<Renderer>().material = worlds[planet].planetMat;
        }
        foreach (GameObject item in planetsInfo)
        {
            item.SetActive(false);
        }
        worlds[planet].info.SetActive(true);
    }
    
    public void BackToMap()
    {
        infoCanvas.SetActive(false);
        planetMap.SetActive(true);
        mainCamera.enabled = true;
        infoCamera.enabled = false;
    }

    public void BackToAR()
    {
        planetMap.SetActive(false);
        ARMain.SetActive(true);
        if (cameraState == CameraState.AR)
        {
            headerAR.SetActive(true);
        }
        
    }

    public void ToVRFromInfo()
    {
        if (IsFreePlanet(currentInfoWorld))
        {
            foreach (World item in worlds)
            {
                item.world.SetActive(false);
            }
            worldsGO.transform.position = mainCamera.gameObject.transform.position;
            cameraState = CameraState.VR;
            LandingCanvas.SetActive(false);
            LeavingCanvas.SetActive(true);
            solarSystem.SetActive(false);
            headerAR.SetActive(false);
            if (infoCanvas.activeInHierarchy)
            {
                GetWorld(currentInfoWorld).world.SetActive(true);
                Debug.Log("name of currentinfo is: " + GetWorld(currentInfoWorld).name);
                planetNameVR.text = GetWorld(currentInfoWorld).name;
                RenderSettings.skybox = GetWorld(currentInfoWorld).sky;
            }
            else
            {
                GetWorld(currentWorld).world.SetActive(true);
                RenderSettings.skybox = GetWorld(currentWorld).sky;
            }
            BGRenderer.enabled = false;
            SetLayer(0);
            mainCamera.enabled = true;
            infoCamera.enabled = false;
            ARMain.SetActive(true);
            infoCanvas.SetActive(false);
        }
        else
        {
            buyFullUIinfo.SetActive(true);
        }
    }

    public void Animate()
    {
        solarSytemFadeIn.orbiting = !solarSytemFadeIn.orbiting;
    }

    public void TrackingState(bool tracking)
    {
        trackingLost.SetActive(!tracking);
    }

    public void VisitNasaWWW()
    {
        string url = "https://solarsystem.nasa.gov/planets/";
        switch (currentInfoWorld)
        {
            case ShowWorld.sun:
                url += "overview/";
                break;
            case ShowWorld.mercury:
                url += "mercury/overview/";
                break;
            case ShowWorld.venus:
                url += "venus/overview/";
                break;
            case ShowWorld.mars:
                url += "mars/overview/";
                break;
            case ShowWorld.earth:
                url += "earth/overview/";
                break;
            case ShowWorld.moon:
                url = "https://solarsystem.nasa.gov/moons/earths-moon/overview/";
                break;
            case ShowWorld.jupiter:
                url += "jupiter/overview/";
                break;
            case ShowWorld.saturn:
                url += "saturn/overview/";
                break;
            case ShowWorld.uranus:
                url += "uranus/overview/";
                break;
            case ShowWorld.neptune:
                url += "neptune/overview/";
                break;
            case ShowWorld.ISS:
                url = "https://www.nasa.gov/audience/forstudents/k-4/stories/nasa-knows/what-is-the-iss-k4.html";
                break;
            case ShowWorld.none:
                url += "overview/";
                break;
            default:
                url += "overview/";
                break;
        }
        Application.OpenURL(url);
    }

    public void VisitWebsite(string url)
    {
        Application.OpenURL(url);
    }

    private bool IsFreePlanet(ShowWorld world)
    {
        bool isfree = true;
        switch (world)
        {
            case ShowWorld.sun:
                break;
            case ShowWorld.mercury:
                break;
            case ShowWorld.venus:
                isfree = true;
                break;
            case ShowWorld.mars:
                isfree = true;
                break;
            case ShowWorld.earth:
                isfree = true;
                break;
            case ShowWorld.moon:
                isfree = true;
                break;
            case ShowWorld.jupiter:
                break;
            case ShowWorld.saturn:
                break;
            case ShowWorld.uranus:
                break;
            case ShowWorld.neptune:
                break;
            case ShowWorld.ISS:
                break;
            case ShowWorld.none:
                isfree = true;
                break;
            default:
                break;
        }
        return isfree;
    }

}
