using UnityEngine;
using UnityEngine.SceneManagement;

public class GoStats : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("RoadEndScene");
    }
}
