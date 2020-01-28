using UnityEngine;
using SingletonUtility;

/// <summary>
/// Class to keep track of values that don't change
/// </summary>
public class Constants : SingletonBehaviour<Constants>
{
    /// <summary>
    /// Key to track the current track being played
    /// </summary>
    [HideInInspector]
    public static string CURRENT_TRACK_PPKEY = "currentTrack";
    /// <summary>
    /// key to track the time in seconds the currently playing track is in
    /// </summary>
    [HideInInspector]
    public static string CURRENT_TIME_PPKEY = "currentTime";
    /// <summary>
    /// Key to track whether the QR code has been scanned in the current session
    /// </summary>
    [HideInInspector]
    public static string QR_CODE_SUCCESFUL = "QRCorrect";
    /// <summary>
    /// Key to track whether the videoscene has been loaded in the current session
    /// </summary>
    public static string VIDEO_SEEN = "videoSeen";
    /// <summary>
    /// URL of the zwijsen store website
    /// </summary>
    public static string ZWIJSEN_URL = "https://www.zwijsen.nl/kinderboeken";
    /// <summary>
    /// URL of the developer's personal website
    /// </summary>
    public static string MERIJN_URL = "https://www.merijnkersten.nl";
    
    /// <summary>
    /// Gets called just before the app closes
    /// </summary>
    private void OnApplicationQuit()
    {        
        PlayerPrefs.SetInt(QR_CODE_SUCCESFUL, 0);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Gets called just before the app pauses
    /// </summary>
    /// <param name="pause"></param>
    private void OnApplicationPause(bool pause)
    {
        // saving the app here is nessecary in case of closing the app via native means (in multitasking view), the app pauses and kills without onquit getting called
        PlayerPrefs.SetInt(QR_CODE_SUCCESFUL, 0);
        PlayerPrefs.Save();
    }
}
