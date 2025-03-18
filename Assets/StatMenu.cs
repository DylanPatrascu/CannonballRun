using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatMenu : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        text.SetText("Time = " + StaticData.totalTime.ToString("00.00") + "\n" + "Time Drifting = " + StaticData.timeDrifted.ToString("00.00"));
    }
    public void NextScene()
    {
        SceneManager.LoadScene(StaticData.nextScene);
    }

}
