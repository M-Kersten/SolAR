using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SingletonUtility;
using System;

public class UIFader : SingletonBehaviour<UIFader>
{
    #region InnerClasses
    /// <summary>
    /// Style for Fading
    /// </summary>
    public enum FadeStyle
    {
        In,
        Out,
        InAndOut,
        OutAndIn
    }
    #endregion

    #region Variables
    #region Public
    /// <summary>
    /// Enable smooth fading along animationcurve (hidden in inspector because of editorscript). Set public for editorScript
    /// </summary>
    public bool EnableSmoothFade;
    /// <summary>
    /// Curve to fade along (hidden in inspector because of editorscript). Set public for editorScript
    /// </summary>
    public AnimationCurve FadeCurve;
    #endregion

    #region Editor
    /// <summary>
    /// Set image to fade
    /// </summary>
    [Tooltip("Set image to fade")]
    [SerializeField]
    private Image fadingImg;
    /// <summary>
    /// Start faded out
    /// </summary>
    [Tooltip("Start faded out")]
    [SerializeField]
    private bool startOpaque;
    #endregion

    #region Private
    /// <summary>
    /// Currently active FadeRoutine
    /// </summary>
    private Coroutine fadeRoutine;
    #endregion
    #endregion

    #region Methods
    #region Unity
    /// <summary>
    /// Initializes to Transparent Color
    /// </summary>
    private void Start()
    {
        fadingImg.color = new Color(fadingImg.color.r, fadingImg.color.g, fadingImg.color.b, startOpaque ? 255 : 0);
    }
    #endregion

    #region Public
    /// <summary>
    /// Fades In to Opaque Color
    /// </summary>
    /// <param name="seconds">Duration of Fade</param>
    public void FadeIn(float seconds)
    {
        Fade(FadeStyle.In, seconds);
    }
    /// <summary>
    /// Fades Out to Transparent Color
    /// </summary>
    /// <param name="seconds">Duration of Fade</param>
    public void FadeOut(float seconds)
    {
        Fade(FadeStyle.Out, seconds);
    }

    /// <summary>
    /// Fades out and in while performing an action in the middle
    /// </summary>
    /// <param name="seconds">Seconds the whole fade takes</param>
    /// <param name="pauseSeconds">Extra time in the pause</param>
    /// <param name="action">A function to invoke when faded out</param>
    public void FadeWithAction(float seconds, float pauseSeconds = 0, Action action = null)
    {
        Fade(FadeStyle.OutAndIn, seconds, pauseSeconds, action);
    }

    /// <summary>
    /// Fade image specified in class
    /// </summary>
    /// <param name="style">Type of fade to initiate</param>
    /// <param name="seconds">Seconds the whole fading procces takes (excluding halfway pause time)</param>
    /// <param name="pauseSeconds">Seconds of pause at halfway point, can be null</param>
    /// <returns></returns>
    public void Fade(FadeStyle style, float seconds, float pauseSeconds = 0, Action action = null)
    {
        // half seconds when fading in and out
        if (style == FadeStyle.InAndOut || style == FadeStyle.OutAndIn)
            seconds *= .5f;
        // prevent multiple coroutines running simultaniously
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeImage(style, seconds, pauseSeconds, action));
    }
    #endregion

    #region Private
    /// <summary>
    /// Fades Image to Opaque/Transparent
    /// </summary>
    /// <param name="style">FadeStyle to Fade to</param>
    /// <param name="seconds">Duration of Fade</param>
    /// <param name="pauseSeconds">Amount of time to Pause for when doing In-Out or Out-In</param>
    private IEnumerator FadeImage(FadeStyle style, float seconds, float pauseSeconds = 0, Action action = null)
    {       
        fadingImg.enabled = true;
        // loop over specified seconds
        for (float i = 0; i <= 1; i += UnityEngine.Time.deltaTime / seconds)
        {
            float val = i;
            if (style == FadeStyle.In || style == FadeStyle.InAndOut)
                val = 1 - i;
            if (EnableSmoothFade) // set color with val as alpha along fading curve (making fading in and out smoother)
                fadingImg.color = new Color(fadingImg.color.r, fadingImg.color.g, fadingImg.color.b, FadeCurve.Evaluate(val));
            else // set color with val as alpha in a linear fashion
                fadingImg.color = new Color(fadingImg.color.r, fadingImg.color.g, fadingImg.color.b, val);
            yield return null;
        }
        if (style == FadeStyle.InAndOut || style == FadeStyle.OutAndIn) // Fading twice
        {
            if (action != null)
            {
                action.Invoke();
            }            
            // hold current screen for specified seconds
            if (pauseSeconds > 0)
                yield return new WaitForSecondsRealtime(pauseSeconds);
            // Fade to Result
            fadeRoutine = StartCoroutine(FadeImage(style.Equals(FadeStyle.OutAndIn) ? FadeStyle.In : FadeStyle.Out, seconds, pauseSeconds));
        }
        fadingImg.color = new Color(fadingImg.color.r, fadingImg.color.g, fadingImg.color.b, style.Equals(FadeStyle.In) ? 0 : 1);
        fadingImg.enabled = fadingImg.color.a != 0;
    }
    #endregion
    #endregion
}
