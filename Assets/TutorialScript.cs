using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{

    public void exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
