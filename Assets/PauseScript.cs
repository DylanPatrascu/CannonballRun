using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject car;
    public TMP_Text scrap;

    [SerializeField]
    private bool isPaused;

    [SerializeField] Transition transition;

    void Start()
    {
        //Hide the UI since the game is not paused by default
        PauseGame(false);
    }

    private void Update()
    {
        scrap.SetText(StaticData.scrap.ToString());
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    public void PauseGame(bool paused)
    {
        if (paused)
        {
            //Show the pause menu
            pauseCanvas.SetActive(true);
        }
        else
        {
            //Hide the pause menu
            pauseCanvas.SetActive(false);
        }

        isPaused = paused;

        //Sets the simulation speed of the game. When 0 time is stopped, when 1 time moves at regular speed
        //https://docs.unity3d.com/ScriptReference/Time-timeScale.html
        Time.timeScale = paused ? 0 : 1;
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        transition.TransitionScene(sceneName);
    }

    public void flipCar()
    {
        car.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void LoadMM()
    {
        Time.timeScale = 1;
        transition.TransitionScene("MainMenu");
    }
}
