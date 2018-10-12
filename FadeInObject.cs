using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Orbit
{
    public string name;
    public GameObject orbitLine;
    public float orbitSpeed;
    public GameObject planet;
    public float planetRotateSpeed;
}

[System.Serializable]
public enum ARLayer
{
    none,
    goldilocks,
    belts
}

public class FadeInObject : MonoBehaviour {
    public GameObject[] layers;   
    public GameObject goldilocksLayer;
    public GameObject beltsLayer;
    public GameObject ISS;

    private ViewPlanet viewPlanet;
    public GameObject[] dissolvingObjects;
    public LineRenderer[] orbitLines;

    public Orbit[] orbits;

    private Color transparent = new Color(255, 255, 255, 0);

    public float fadeSpeed;
    public float orbitSpeed;
    public float planetRotationSpeed;

    private float time;
    
    public bool orbiting;
    private float lineWidth;

    public AnimationCurve positionAnim;
    public AnimationCurve scaleAnim;
    public AnimationCurve lineWidthAnim;

    private bool animationFinish;

    private void Awake()
    {
        transform.localScale = new Vector3(scaleAnim.Evaluate(0), scaleAnim.Evaluate(0), scaleAnim.Evaluate(0));
        transform.localPosition = new Vector3(0, positionAnim.Evaluate(0), 0);
        time = 0;
        lineWidth = orbitLines[0].startWidth;
        animationFinish = true;
        foreach (GameObject item in dissolvingObjects)
        {
            item.GetComponent<MeshRenderer>().material.SetFloat("_Fill", 0);
        }
        ISS.SetActive(false);
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
            foreach (GameObject item in dissolvingObjects)
            {
                item.GetComponent<MeshRenderer>().material.SetFloat("_Fill", time * fadeSpeed);
            }
            foreach (LineRenderer item in orbitLines)
            {
                item.startWidth = lineWidthAnim.Evaluate(time) * lineWidth;
                item.endWidth = lineWidthAnim.Evaluate(time) * lineWidth;
            }
            transform.localPosition = new Vector3(0, positionAnim.Evaluate(time), 0);
            transform.localScale = new Vector3((scaleAnim.Evaluate(time) * .1f) + .9f, (scaleAnim.Evaluate(time) * .1f) + .9f, (scaleAnim.Evaluate(time) * .1f) + .9f);    
        }
        else if(animationFinish)
        {
            ISS.SetActive(true);
            foreach (GameObject item in dissolvingObjects)
            {
                item.GetComponent<MeshRenderer>().material.SetVector("_Noisespeed", new Vector4(0,0,0,0));
            }
            animationFinish = false;
        }

        if (orbiting)
        {
            foreach (Orbit item in orbits)
            {
                item.orbitLine.transform.Rotate(Vector3.up * Time.deltaTime * -orbitSpeed / item.orbitSpeed, Space.Self);
                item.planet.transform.Rotate(Vector3.up * Time.deltaTime * -planetRotationSpeed * item.planetRotateSpeed, Space.Self);
            }
        }
    }

    public void SetLayer(int layer)
    {
        ARLayer aRlayer = (ARLayer)layer;
        foreach (GameObject item in layers)
        {
            item.SetActive(false);
        }        
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
