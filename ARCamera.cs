using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent (typeof(Camera))]
[RequireComponent(typeof(ARCameraBackground))]
public class ARCamera : MonoBehaviour
{
    public static ARCamera Instance => instance;
    private static ARCamera instance;

    public CameraState cameraState;

    private ARCameraBackground cameraBackground;
    private Transform cameraTransform;
    private Camera attachedCamera;

    private void Awake()
    {
        instance = this;
        cameraTransform = GetComponent<Transform>();
        attachedCamera = GetComponent<Camera>();
        cameraBackground = GetComponent<ARCameraBackground>();
    }

    public Transform CameraTransform => cameraTransform;
    public Camera Camera => attachedCamera;

    public void SetCameraBackgroundFromMenu(bool active)
    {
        UIFader.Instance.FadeWithAction(.5f, action: () => cameraBackground.enabled = active);
    }

    public void SetCameraBackground(bool active)
    {
        cameraBackground.enabled = active;
    }
}
