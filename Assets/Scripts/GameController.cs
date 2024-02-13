using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public Image blackSquare;

    public float timeLeft = 300.0f;
    public TextMeshProUGUI timerText;
    public int maxEnemies = 10;
    public int playerLevel = 1;
    public GameObject gameControlsUi;
    private int currentEnemies = 0;
    public GameObject RoguePrefab;
    public CinemachineVirtualCamera cinemachineCam;

    private static readonly bool rogueInstantiated = false;

    private void Awake()
    {
        // Singleton check
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Register the scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Debug.Log("Additional GameController instance found and destroyed");
            Destroy(gameObject);
            return;
        }

        gameControlsUi.SetActive(true);
        _ = StartCoroutine(UpdateTimer());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        blackSquare.color = new Color(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, 1f);
        _ = StartCoroutine(HandleSceneSetup());
    }

    private IEnumerator HandleSceneSetup()
    {
        yield return new WaitForSeconds(0.1f); // Wait a bit to ensure the scene is hidden

        if (GameObject.FindObjectOfType<RogueSkillController>() == null)
        {
            GameObject rogueInstance = Instantiate(RoguePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            cinemachineCam.Follow = rogueInstance.transform;
            cinemachineCam.m_Lens.FieldOfView = 60f;
        }

        _ = StartCoroutine(DelayedFade());
    }

    private IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(0.9f); // Wait to make it a total of 1 second with the HandleSceneSetup delay
        _ = StartCoroutine(FadeBlackInSquare());
    }


    public IEnumerator FadeBlackInSquare(float fadeSpeed = 0.2f)
    {
        Color objectColor = blackSquare.color;
        float fadeAmount;

        while (blackSquare.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackSquare.color = objectColor;
            yield return null;
        }
    }

    private void Start()
    {
        _ = StartCoroutine(UpdateTimer());
    }

    private void Update()
    {
        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        currentEnemies++;
    }

    private IEnumerator UpdateTimer()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1.0f);
            timeLeft--;
            _ = Mathf.FloorToInt(timeLeft / 60);
            _ = Mathf.FloorToInt(timeLeft % 60);
            //timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        EndGame();
    }

    private void EndGame()
    {
        // Game over logic here.
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
