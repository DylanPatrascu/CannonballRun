using System.Collections;
using System.Collections.Generic;
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
        transition.Play("Exit");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

}
