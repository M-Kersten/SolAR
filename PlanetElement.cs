using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlanetElement : MonoBehaviour
{
    
    [SerializeField]
    private Location location;

    private PlanetCarousel carousel;

    public Location Planet => location;

    void Start()
    {
        carousel = GetComponentInParent<PlanetCarousel>();
        GetComponent<Button>().onClick.AddListener(FocusOnPlanet);
    }

    private void FocusOnPlanet()
    {
        if (carousel == null)
            return;

        carousel.FocusOnPlanet(.2f, (RectTransform)transform);

    }

}
