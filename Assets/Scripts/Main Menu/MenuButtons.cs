using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public void StartGame()
    {
        StaticData.upgrades = new List<UpgradeData>();
        StaticData.areas = new List<string>();
        StaticData.scrap = 200;
        StaticData.totalTime = 0;
        StaticData.timeDrifted = 0;
        StaticData.distanceTraveled = 0;
        StaticData.enemiesKilled = 0;
        StaticData.section = 1;
        StaticData.currentNode = 0;
        StaticData.treeGen = false;
        StaticData.alive = true;
        SceneManager.LoadScene("GameScene");
        StaticData.runTime = 0;
    }

    public void OpenCollection()
    {

    }

    public void OpenSettings()
    {

    }

    public void Quit()
    {

#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

}
