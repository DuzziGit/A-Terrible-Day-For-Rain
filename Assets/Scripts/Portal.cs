using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public int sceneIndex = 0;
    public Image blackSquare;
    private Vector3 teleportPosition = new(-103f, 103f, 2f);

    private void Start()
    {
        // Add the fade-in as an event when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade back in when a new scene is loaded
        _ = StartCoroutine(FadeBlackOutSquare(false, 1));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //  Debug.Log("Object entered the portal's trigger.");

        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player is inside the portal's trigger.");

            if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Attempting to fade out.");
                _ = StartCoroutine(FadeAndLoadScene(collision));
            }
        }
    }

    private IEnumerator FadeAndLoadScene(Collider2D playerCollider)
    {
        yield return StartCoroutine(FadeBlackOutSquare(true, 1));

        // After fade-out is complete, wait for the specified time
        yield return new WaitForSeconds(1f);

        // Teleport the player
        RogueSkillController rogue = playerCollider.GetComponent<RogueSkillController>();
        if (rogue != null)
        {
            rogue.transform.position = teleportPosition;
        }

        // Then, load the new scene. Once the new scene is loaded,
        // the OnSceneLoaded event will automatically trigger the fade-in.
        Debug.Log("Attempting to load scene.");
        SceneManager.LoadScene(sceneIndex);
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 2)
    {
        Debug.Log("Fade");
        Color objectColor = blackSquare.color;
        float fadeAmount;

        if (fadeToBlack)
        {
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
        // Important to unsubscribe the event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
