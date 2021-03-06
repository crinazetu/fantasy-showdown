using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public string sceneName;


    public void Start()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //Play Animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }



    public void loadscene(string scenename)
    {

        StartCoroutine(LoadLevel(scenename));
    }
    IEnumerator LoadLevel(string sceneName)
    {
        //Play Animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }

}
