using UnityEngine;
using UnityEngine.SceneManagement;

public class StatMenu : MonoBehaviour
{
    
    public void NextScene()
    {
        SceneManager.LoadScene(StaticData.nextScene);
    }

}
