using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMain : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
