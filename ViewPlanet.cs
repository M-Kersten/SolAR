using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ViewPlanet : MonoBehaviour
{
    public static ViewPlanet Instance => instance;
    private static ViewPlanet instance;

    [HideInInspector]
    public float SolarSystemScale;

    [Header("Scene")]
    public Material milkyWayBackground;
    public Location currentViewingLocation = Location.none;

    public GameObject solarSystem;

    [Header("UI")]
    public PlanetCarousel planetCarousel;
    public Button PlanetMapButton;
    public Button ClosePlanetMapButton;
    public Button LandHereButton;
    public Button ScaleButton;

    public GameObject planetMap;
    public Text planetName;
    public Text planetNameVR;
    public GameObject LandingCanvas;
    public GameObject LeavingCanvas;
    public GameObject headerAR;
    public GameObject ARMain;
    public CanvasGroup trackingLost;
    public GameObject buyFullUI;
    public GameObject buyFullUIinfo;

    public GameObject[] layerCanvas;

    [HideInInspector]
    public FadeInObject solarSytemFadeIn;

    #region init
    void Awake()
    {
        instance = this;
        PlanetMapButton.onClick.AddListener(() => SetPlanetMap(true));
        ClosePlanetMapButton.onClick.AddListener(() => ToAR());
        LandHereButton.onClick.AddListener(ToVRScene);
        ScaleButton.onClick.AddListener(() => SlideUI.Instance.Slide(false, ()=> UIManager.Instance.SetScalingUI(true)));
        ARSession.stateChanged += TrackingState;
        currentViewingLocation = Location.none;
    }
    #endregion

    #region ChangeXR
    private void SetPlanetMap(bool active)
    {
        UIFader.Instance.FadeWithAction(.5f, action: () =>
        {
            planetMap.SetActive(active);
            ARMain.SetActive(!active);
        });
    }

    public void ToVRScene()
    {
        if (!IsFreePlanet(currentViewingLocation) && !GameSettings.Instance.FullVersion)
        {
            if (planetMap.activeInHierarchy)
            {
                buyFullUIinfo.SetActive(true);
                Debug.Log("buy full popup shown");
            }
            else
                buyFullUI.SetActive(true);

            return;
        }        
        planetCarousel.ZoomOnPlanet(() => 
        {
            ARCamera.Instance.cameraState = CameraState.VR;
            ARCamera.Instance.SetCameraBackground(false);
            planetMap.SetActive(false);
            LandingCanvas.SetActive(false);
            LeavingCanvas.SetActive(true);
            solarSystem.SetActive(false);
            ARMain.SetActive(false);
            headerAR.SetActive(false);
            PlanetManager.Instance.SpawnPlanetEnvironment(currentViewingLocation);
            planetNameVR.text = PlanetManager.Instance.World(currentViewingLocation).PresetWorld.Name;
            SetLayer(0);
        });
        
    }

    public void ToAR()
    {
        UIFader.Instance.FadeWithAction(.4f, action: () =>
        {
            ARCamera.Instance.cameraState = CameraState.AR;
            RenderSettings.skybox = milkyWayBackground;
            LandingCanvas.SetActive(false);
            LeavingCanvas.SetActive(false);
            solarSystem.SetActive(true);
            planetMap.SetActive(false);
            headerAR.SetActive(true);
            ARMain.gameObject.SetActive(true);
            PlanetManager.Instance.SpawnPlanetEnvironment(Location.none);
        });
    }

    public void ToPlanetMap()
    {
        UIFader.Instance.FadeWithAction(.5f, action: () =>
        {
            PlanetManager.Instance.SpawnPlanetEnvironment(Location.none);
            LeavingCanvas.SetActive(false);           
            ARMain.SetActive(false);
            planetMap.SetActive(true);
            RenderSettings.skybox = milkyWayBackground;
        });
    }
    #endregion

    #region GetSetVariables

    public void SetPlanet(Location world)
    {
        currentViewingLocation = world;
        planetName.text = PlanetManager.Instance.World(world).PresetWorld.Name;
        planetNameVR.text = PlanetManager.Instance.World(world).PresetWorld.Name;
        LandingCanvas.SetActive(world != Location.none);
    }

    public void ResetPlanet()
    {
        if (ARCamera.Instance.cameraState == CameraState.AR)
            currentViewingLocation = Location.none;
        LandingCanvas.SetActive(false);
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

    private bool IsFreePlanet(Location world)
    {
        bool isfree = false;
        isfree = GameSettings.Instance.FullVersion;
        switch (world)
        {
            case Location.sun:
                break;
            case Location.mercury:
                break;
            case Location.venus:
                isfree = true;
                break;
            case Location.mars:
                isfree = true;
                break;
            case Location.earth:
                isfree = true;
                break;
            case Location.moon:
                isfree = true;
                break;
            case Location.jupiter:
                break;
            case Location.saturn:
                break;
            case Location.uranus:
                break;
            case Location.neptune:
                break;
            case Location.ISS:
                break;
            case Location.none:
                isfree = true;
                break;
            default:
                break;
        }
        return isfree;
    }

    #endregion

    public void DisableText() => LandingCanvas.SetActive(false);

    public void Animate() => solarSytemFadeIn.orbiting = !solarSytemFadeIn.orbiting;

    public void TrackingState(ARSessionStateChangedEventArgs sessionState)
    {
        bool tracking = sessionState.state == ARSessionState.SessionTracking;
        trackingLost.alpha = tracking ? 0 : 1;

        if (tracking)
            trackingLost.gameObject.SetActive(true);

        LeanTween.alphaCanvas(trackingLost, tracking ? 1 : 0, .75f).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
        {
            if (tracking)
                trackingLost.gameObject.SetActive(false);
        });
    }
}
