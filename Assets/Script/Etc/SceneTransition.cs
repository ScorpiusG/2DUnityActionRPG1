using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition transition;
    public Animator mAnimator;
    public string stringSceneName = "";

    private void Awake()
    {
        if (transition == null)
        {
            transition = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void GoToScene(string sceneName)
    {
        if (transition.mAnimator != null)
        {
            transition.stringSceneName = sceneName;
            transition.mAnimator.Play("transition");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void ChangeSceneNow()
    {
        SceneManager.LoadScene(transition.stringSceneName);
    }
}