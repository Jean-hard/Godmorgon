﻿using GodMorgon.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMgr : MonoBehaviour
{
    public GameObject playButton = null;
    public GameObject musicManagerObject = null;

    public float waitTime = 2f;


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
        musicManagerObject.GetComponent<MusicTitleScreen>().StopMusic();
        //Destroy(musicManagerObject);
        //Destroy(GameObject.Find("WwiseGlobal"));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Original_Scene");
    }
}
