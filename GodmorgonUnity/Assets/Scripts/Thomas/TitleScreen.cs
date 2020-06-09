using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour
{

    float waitTimer = 5f;

    public void Play()
    {
        StartCoroutine(LaunchGame());
    }

    IEnumerator LaunchGame()
    {
        yield return new WaitForSeconds(waitTimer);
        SceneManager.LoadScene("Original_Scene");
    }
}