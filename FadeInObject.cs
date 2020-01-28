using UnityEngine;

public class FadeInObject : MonoBehaviour
{
    public GameObject[] layers;   
    public GameObject goldilocksLayer;
    public GameObject beltsLayer;
    public GameObject ISS;
    public GameObject saturnRing;
    public GameObject Sun;
    public Material sunMat;

    public MeshRenderer[] dissolvingObjects;
    public LineRenderer[] orbitLines;

    public Orbit[] orbits;
    
    public float fadeSpeed;
    public float orbitSpeed;
    public float planetRotationSpeed;
    
    public bool orbiting;

    public AnimationCurve positionAnim;
    public AnimationCurve scaleAnim;
    public AnimationCurve lineWidthAnim;

    private float time;
    private float lineWidth;
    private ViewPlanet viewPlanet;
    private bool animationFinish;

    private void Awake()
    {
        transform.localScale = new Vector3(scaleAnim.Evaluate(0), scaleAnim.Evaluate(0), scaleAnim.Evaluate(0));
        transform.localPosition = new Vector3(transform.localPosition.x, positionAnim.Evaluate(0), transform.localPosition.z);
        time = 0;
        lineWidth = orbitLines[0].startWidth;
        animationFinish = true;
        foreach (MeshRenderer mesh in dissolvingObjects)
        {
            mesh.material.SetFloat("_Fill", 0);
        }
        ISS.SetActive(false);
        saturnRing.SetActive(false);
        foreach (Orbit item in orbits)
        {
            if (item.orbitLine.GetComponent<LineRenderer>() != null)
            {
                item.orbitLine.GetComponent<LineRenderer>().startWidth = 0;
                item.orbitLine.GetComponent<LineRenderer>().endWidth = 0;
            }
        }
    }

    void Update()
    {
        if (time * fadeSpeed < 1)
        {
            time += Time.deltaTime;
            for (int i = 0; i < dissolvingObjects.Length; i++)
            {
                MeshRenderer mesh = dissolvingObjects[i];
                mesh.material.SetFloat("_Fill", time * fadeSpeed);
            }
            foreach (LineRenderer item in orbitLines)
            {
                item.startWidth = lineWidthAnim.Evaluate(time) * lineWidth;
                item.endWidth = lineWidthAnim.Evaluate(time) * lineWidth;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, positionAnim.Evaluate(time), transform.localPosition.z);
            transform.localScale = new Vector3((scaleAnim.Evaluate(time) * .1f) + .9f, (scaleAnim.Evaluate(time) * .1f) + .9f, (scaleAnim.Evaluate(time) * .1f) + .9f);    
        }
        else if(animationFinish)
        {
            ISS.SetActive(true);
            saturnRing.SetActive(true);
            Sun.GetComponent<Renderer>().material = sunMat;
            for (int i = 0; i < dissolvingObjects.Length; i++)
            {
                MeshRenderer mesh = dissolvingObjects[i];
                mesh.material.SetVector("_Noisespeed", new Vector4(0,0,0,0));
            }
            animationFinish = false;
        }

        if (orbiting)
        {
            for (int i = 0; i < orbits.Length; i++)
            {
                Orbit item = orbits[i];
                item.orbitLine.transform.Rotate(Vector3.up * Time.deltaTime * -orbitSpeed / item.orbitSpeed, Space.Self);
                item.planet.transform.Rotate(Vector3.up * Time.deltaTime * -planetRotationSpeed * item.planetRotateSpeed, Space.Self);
            }
        }
    }

    public void SetLayer(int layer)
    {
        ARLayer aRlayer = (ARLayer)layer;
        for (int i = 0; i < layers.Length; i++)
            layers[i].SetActive(false);

        switch (aRlayer)
        {
            case ARLayer.none:
                break;
            case ARLayer.goldilocks:
                goldilocksLayer.SetActive(true);
                break;
            case ARLayer.belts:
                beltsLayer.SetActive(true);
                break;
            default:
                break;
        }
    }
}
