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
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
        /*
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            */
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }
    /*
        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }*/
    }