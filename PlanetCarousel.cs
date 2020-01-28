using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlanetCarousel : ScrollRect
{
    [SerializeField]
    protected Camera planetSelectionCamera;
    [SerializeField]
    protected AnimationCurve scaleCurve;
    [SerializeField]
    protected Text planetNameText;
    [SerializeField]
    protected PlanetInfo planetInfo;
    [SerializeField]
    protected float zoomDistance;

    private Dictionary<Location, RectTransform> carouselLocations = new Dictionary<Location, RectTransform>();
    private CanvasGroup planetNameCanvasGroup;
    private float globalMouseDelta;

    protected override void Awake()
    {
        base.Awake();
        foreach (Transform child in content)
        {
            carouselLocations.Add(child.GetComponent<PlanetElement>() ? child.GetComponent<PlanetElement>().Planet: Location.none, (RectTransform)child);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        planetNameCanvasGroup = planetNameText.gameObject.GetComponent<CanvasGroup>();
        if (ViewPlanet.Instance.currentViewingLocation != Location.none)
            FocusOnPlanet(.1f, carouselLocations[ViewPlanet.Instance.currentViewingLocation]);
    }

    private void Update()
    {
        ScaleObjects();
    }
    
    private void ScaleObjects()
    {
        float maxDisplacement = content.rect.width / 2;        
        foreach (Transform child in content)
        {            
            //Work out values for calculating carrousel ScaleAndPosition.
            float distFromCentre = child.InverseTransformPoint(viewport.transform.position).x;
            float displacementFromCentre = Mathf.Abs(distFromCentre);
            float normalisedDisplacement = displacementFromCentre / maxDisplacement;

            //Get desired values of scale and position from animation curves.
            float animationScale = scaleCurve.Evaluate(normalisedDisplacement);
            if (child.gameObject.name != "Sun")
            {
                child.gameObject.transform.localScale = Vector3.one * animationScale;
            }
        }
    }
    
    public void FocusOnPlanet(float duration, RectTransform setFocusElement = null)
    {
        RectTransform focusedElement = setFocusElement ?? GetFocussedElement();

        if (focusedElement.gameObject.tag == "sun")
            focusedElement = (RectTransform)content.transform.GetChild(1).transform;

        planetNameText.text = focusedElement.gameObject.name;        
        velocity = Vector2.zero;

        float startPosition = content.anchoredPosition.x;
        float endPosition = -focusedElement.localPosition.x;
        LeanTween.value(content.gameObject, startPosition, endPosition, duration)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnUpdate((value) => content.anchoredPosition = new Vector2(value, content.anchoredPosition.y))
            .setOnComplete(() => velocity = Vector2.zero);
        LeanTween.alphaCanvas(planetNameCanvasGroup, 1, duration);

        ViewPlanet.Instance.currentViewingLocation = focusedElement.GetComponent<PlanetElement>().Planet;
        planetInfo.Display(ViewPlanet.Instance.currentViewingLocation);
    }

    private RectTransform GetFocussedElement()
    {
        float maxScale = 0;
        RectTransform focused = null;

        foreach (Transform child in content)
        {
            if (child.localScale.x > maxScale)
            {
                maxScale = child.localScale.x;
                focused = (RectTransform)child;
            }
        }
        return focused;    
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        LeanTween.cancelAll();
        LeanTween.alphaCanvas(planetNameCanvasGroup, 0, .3f);
        planetInfo.Hide();
    }

    public void ZoomOnPlanet(Action callback = null)
    {
        planetInfo.Hide();
        float startingPosition = planetSelectionCamera.orthographicSize;
        LeanTween.value(gameObject, startingPosition, startingPosition - zoomDistance, 1f)
            .setOnUpdate((value) => planetSelectionCamera.orthographicSize = value)
            .setEase(LeanTweenType.easeInOutQuart);

        UIFader.Instance.FadeWithAction(2, action: () => 
            {
                LeanTween.cancel(gameObject);
                planetSelectionCamera.orthographicSize = startingPosition;
                callback?.Invoke();
            });        
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        FocusOnPlanet(.2f);
    }

}
