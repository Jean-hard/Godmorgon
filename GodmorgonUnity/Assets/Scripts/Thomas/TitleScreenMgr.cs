using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMgr : MonoBehaviour
{
    public GameObject playButton = null;
    public GameObject musicManagerObject = null;

    public float waitTime = 5f;


    /**
     * Appelée par le button play
     */
    public void Play()
    {
        StartCoroutine(LaunchGame());
    }

    /**
     * Change de scène après tant de secondes
     */
    IEnumerator LaunchGame()
    {
        playButton.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        Destroy(musicManagerObject);
        SceneManager.LoadScene("Original_Scene");
    }
}
