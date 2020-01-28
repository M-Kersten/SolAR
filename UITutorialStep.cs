using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UITutorialStep : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void EaseIn(float duration, bool forward)
    {
        gameObject.SetActive(true);
        gameObject.transform.localPosition = new Vector3(forward ? 100 : -100, 0, 0);
        LeanTween.moveLocalX(gameObject, 0, duration).setEase(LeanTweenType.easeOutQuart);
        LeanTween.alphaCanvas(canvasGroup, 1, duration).setEase(LeanTweenType.easeInOutSine);
    }

    public void EaseOut(float duration, Action callback = null)
    {
        LeanTween.alphaCanvas(canvasGroup, 0, duration).setEase(LeanTweenType.easeInOutSine).setOnComplete(() => 
        {
            gameObject.SetActive(false);
            callback?.Invoke();
        });
    }
}
