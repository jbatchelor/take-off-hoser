using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip landedSound;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem landedParticles;

    Movement _moveScript;
    AudioSource _audio;

    bool isTransitioning = false;
    bool collisionDisabled = false;


    void Start()
    {
        _moveScript = GetComponent<Movement>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) return;

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
        landedParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        _audio.Stop();
        _audio.PlayOneShot(explosionSound);
        _moveScript.enabled = false;
        explosionParticles.Play();
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
