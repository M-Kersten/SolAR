using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public GameObject spawnMenu;
    public GameObject mainMenu;
    public GameObject spawnPoint;
    public ARController aRController;
    public GameObject pointCloud;

    public void SpawnSolarSystem()
    {
        aRController.SpawnSolarSystem();
        spawnPoint.SetActive(false);
        spawnMenu.SetActive(false);
        mainMenu.SetActive(true);
        pointCloud.SetActive(false);
    }

    public void ResetSolarSystem()
    {
        ViewPlanet.instance.SetLayer(0);
        aRController.ResetActiveObjects();
        spawnPoint.SetActive(true);
        spawnMenu.SetActive(true);
        mainMenu.SetActive(false);
        pointCloud.SetActive(true);
    }


    public void QuitApp()
    {
        Application.Quit();
    }

    public void ViewFullVersion()
    {
        Application.OpenURL("market://details?id=com.MKProductions.SolAR");
    }
    
}
