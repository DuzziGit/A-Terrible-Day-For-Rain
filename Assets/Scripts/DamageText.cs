using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float displayTime = 1.5f;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        //  Debug.Log("Text Component: " + textComponent);
    }

    private void Start()
    {
        StartCoroutine(ShowAndDestroy());
    }

    IEnumerator ShowAndDestroy()
    {
        yield return new WaitForSeconds(displayTime);
        Destroy(gameObject);
    }

    // Set the damage number on the TMP_Text component
    public void SetDamageNumber(int damage)
    {
        if (textComponent != null)
        {
            textComponent.text = damage.ToString();
        }
    }
}
