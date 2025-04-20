using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private Animator transition;

    private void Start()
    {
        transition = GetComponentInChildren<Animator>();
    }

    public void TransitionScene(string scene)
    {
        StartCoroutine(ExecuteTransition(scene));
    }

    private IEnumerator ExecuteTransition(string scene)
    {
        Debug.Log("transition");
        transition.Play("Exit");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(scene);
    }

}
