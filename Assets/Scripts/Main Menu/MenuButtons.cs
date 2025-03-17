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
        StaticData.scrap = 0;
        SceneManager.LoadScene("GameScene");
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
