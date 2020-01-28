using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class SceneLoader : MonoBehaviour {

    /// <summary>
    /// The main scene that gets loaded asynchronously
    /// </summary>
    [SerializeField]
    private string startSceneName;

    /// <summary>
    /// The main scene that gets loaded asynchronously
    /// </summary>
    [SerializeField]
    private string mainSceneName;
    
    private void Start()
    {
        UIFader.Instance.FadeIn(1.5f);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitApplication()
    {
        LeanTween.cancelAll();
        UIFader.Instance.Fade(UIFader.FadeStyle.Out, 1, 0, () => Application.Quit());        
    }
    
    public void LoadMainScene(GameObject permissionWindow = null)
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            if (permissionWindow != null)
                permissionWindow.SetActive(true);
            else
                Permission.RequestUserPermission(Permission.Camera);

            return;
        }
#endif

        LeanTween.cancelAll();
        UIFader.Instance.FadeWithAction(.5f, 0, () => SceneManager.LoadScene(mainSceneName));
    }

    public void LoadIntroScene()
    {
        LeanTween.cancelAll();
        UIFader.Instance.FadeWithAction(.5f, 0, () => SceneManager.LoadScene(startSceneName));
    }
}
