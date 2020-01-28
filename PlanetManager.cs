using UnityEngine;
using SingletonUtility;
using System.Linq;

public class PlanetManager : MonoBehaviour
{
    #region Singleton instance
    private static PlanetManager instance;
    public static PlanetManager Instance => instance;
    #endregion


    public WorldPreset[] worlds;

    private GameObject activeEnvironment;


    private void Awake() => instance = this;

    public WorldPreset World(Location location)
    {
        foreach (WorldPreset preset in worlds.Where(x => x.PresetWorld.Location == location).Select(x => x))
            return preset;
        return null;
    }
    public void SpawnPlanetEnvironment(Location location)
    {
        if (activeEnvironment != null && activeEnvironment.activeInHierarchy)
            Destroy(activeEnvironment);

        if (location == Location.none)
            return;

        World newWorld = World(location).PresetWorld;

        activeEnvironment = Instantiate(newWorld.Environment, ARCamera.Instance.CameraTransform.position + Vector3.down * 2.5f, Quaternion.identity, transform);
        RenderSettings.skybox = newWorld.SkyBox;

    }

}
