using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadEnd : MonoBehaviour
{
    public GameObject player;
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            UpdateScene("RoadEndScene");
        }
    }

    public void UpdateScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
