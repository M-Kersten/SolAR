using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetData - ", menuName = "Planet data", order = 1)]
public class PlanetData : ScriptableObject
{
    public Location world;
    [TextArea(20,100)]
    public string leftData;
    [TextArea(20, 100)]
    public string rightData;
}
