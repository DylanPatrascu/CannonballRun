using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] List<UpgradeData> startingUpgrades;
    [SerializeField] Transition transition;

    public void StartGame()
    {
        StaticData.upgrades = new List<UpgradeData>();
        foreach (UpgradeData upgrade in startingUpgrades) StaticData.upgrades.Add(upgrade);
        StaticData.areas = new List<string>();
        StaticData.scrap = StaticData.startingScrap;
        StaticData.totalTime = 0;
        StaticData.timeDrifted = 0;
        StaticData.distanceTraveled = 0;
        StaticData.enemiesKilled = 0;
        StaticData.section = 1;
        StaticData.currentNode = 0;
        StaticData.treeGen = false;
        StaticData.alive = true;
        StaticData.runTime = 0;
        transition.TransitionScene("GameScene");
        
    }
    public void OpenUpgrades()
    {
        transition.TransitionScene("UpgradeScene");
    }

    public void OpenTutorial()
    {
        transition.TransitionScene("TutorialScene");
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
