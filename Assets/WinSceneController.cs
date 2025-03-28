using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{

    public TextMeshProUGUI text;
    void Start()
    {
        StaticData.runTime += StaticData.totalTime;
        text.SetText("Time\n" + StaticData.runTime.ToString("00.00"));
    }

    public void goMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
