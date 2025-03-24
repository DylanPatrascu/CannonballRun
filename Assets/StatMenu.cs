using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatMenu : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        text.SetText("Time\n" + StaticData.totalTime.ToString("00.00") + "\n" + "Time Drifting\n" + StaticData.timeDrifted.ToString("00.00"));
    }
    public void NextScene()
    {
        if (StaticData.alive)
        {
            SceneManager.LoadScene(StaticData.nextScene);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
