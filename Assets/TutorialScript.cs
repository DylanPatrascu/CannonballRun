using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{

    [SerializeField] private Transition transition;

    public void exit()
    {
        transition.TransitionScene("MainMenu");
    }
}
