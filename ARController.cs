using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using GoogleARCore;
using System.Collections.Generic;

public class ARController : MonoBehaviour
{
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public GameObject FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject[] ObjectPrefab;

        public GameObject solarSystem;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        //private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

    private List<GameObject> activeObjects = new List<GameObject>();
       
    public void SpawnObject(int number)
    {
        Vector3 objectPos = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z);
        Pose cameraPose;
        cameraPose.position = objectPos;
        cameraPose.rotation = FirstPersonCamera.transform.localRotation;
        var newObject = Instantiate(ObjectPrefab[number], objectPos, FirstPersonCamera.transform.localRotation);
        var objectAnchor = Session.CreateAnchor(cameraPose);        
        newObject.transform.parent = objectAnchor.transform;
        activeObjects.Add(newObject);
    }

    public void SpawnSolarSystem()
    {
        Vector3 objectPos = new Vector3(FirstPersonCamera.transform.position.x, FirstPersonCamera.transform.position.y, FirstPersonCamera.transform.position.z);
        Pose cameraPose;
        cameraPose.position = objectPos;
        cameraPose.rotation = FirstPersonCamera.transform.localRotation;
        GameObject newObject = Instantiate(solarSystem, objectPos, FirstPersonCamera.transform.localRotation);
        ViewPlanet.instance.solarSystem = newObject;
        ViewPlanet.instance.solarSytemFadeIn = newObject.GetComponent<FadeInObject>();
        var objectAnchor = Session.CreateAnchor(cameraPose);
        newObject.transform.parent = objectAnchor.transform;
        activeObjects.Add(newObject);
    }

    public void ResetActiveObjects()
    {
        foreach (GameObject item in activeObjects)
        {
            Destroy(item);
        }
        activeObjects.Clear();
    }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }   
    }
