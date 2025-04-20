using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatMenu : MonoBehaviour
{
    public TextMeshProUGUI text;

    [SerializeField] private Transition transition;

    void Start()
    {
        StaticData.runTime += StaticData.totalTime;
        text.SetText("Time\n" + StaticData.totalTime.ToString("00.00") + "\n" + "Time Drifting\n" + StaticData.timeDrifted.ToString("00.00"));
    }
    public void NextScene()
    {
        if (StaticData.alive)
        {
            transition.TransitionScene(StaticData.nextScene);
        }
        else
        {
            transition.TransitionScene("MainMenu");
        }
    }

}
