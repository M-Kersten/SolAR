using UnityEditor;
using UnityEngine;

/// <summary>
/// Class helping with playerprefs in unity editor
/// </summary>
public class ClearPlayerPrefs : MonoBehaviour
{
    /// <summary>
    /// Clears playerprefs by going to tools/clear playerprefs
    /// </summary>
#if UNITY_EDITOR
    [MenuItem("Tools/Clear playerprefs")]
    public static void ClearPrefs()
    {
        Debug.Log("playerprefs cleared");
        PlayerPrefs.DeleteAll();
    }
#endif
}
