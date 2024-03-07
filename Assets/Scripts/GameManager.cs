using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public Image blackSquare;
    public GameObject gameControlsUi;
    public CinemachineVirtualCamera cinemachineCam;
    public static GameManager Instance;
    public PlayerMovement playerMovement;
    public GameObject Player;

    public bool playerCanMove = true;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        FindPlayer();
    }


    public GameObject FindPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        return Player;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        blackSquare.color = new Color(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, 1f);
        _ = StartCoroutine(DelayedFade());

    }
    private IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(0.9f); // Wait to make it a total of 1 second with the HandleSceneSetup delay
        _ = StartCoroutine(FadeBlackInSquare());
    }


    public IEnumerator FadeBlackInSquare(float fadeSpeed = 0.5f)
    {
        playerCanMove = false; // Initially, the player cannot move.
        float pSpeed;
        pSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = 0;
        Color objectColor = blackSquare.color;
        float fadeAmount;

        while (blackSquare.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackSquare.color = objectColor;

            // Enable player movement when alpha is approximately 0.39 or lower, but only do this once
            if (blackSquare.color.a <= 0.39 && !playerCanMove)
            {
                playerMovement.moveSpeed = pSpeed;
                playerCanMove = true; // Player can now move.
            }

            yield return null;
        }
        playerMovement.moveSpeed = pSpeed;
        playerCanMove = true;
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = .2f)
    {
        Color objectColor = blackSquare.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            objectColor.a = 0;
            blackSquare.color = objectColor;

            while (blackSquare.color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (blackSquare.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.color = objectColor;
                yield return null;
            }
        }
    }

    private void OnDestroy()
    {
        // Make sure to unregister the OnSceneLoaded method when this object is destroyed.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
