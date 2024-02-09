using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip landedSound;

    Movement _moveScript;
    AudioSource _audio;

    bool isTransitioning = false;

    void Start()
    {
        _moveScript = GetComponent<Movement>();
        _audio = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Back at the start again!");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        _audio.Stop();
        _audio.PlayOneShot(landedSound);
        _moveScript.enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        _audio.Stop();
        _audio.PlayOneShot(explosionSound);
        _moveScript.enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIdx);
        _moveScript.enabled = true;
        isTransitioning = false;
    }

    void LoadNextLevel()
    {
        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIdx = currSceneIdx + 1;
        if (nextSceneIdx == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIdx = 0;
        }
        SceneManager.LoadScene(nextSceneIdx);
        _moveScript.enabled = true;
        isTransitioning = false;
    }
}
