using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private readonly float displayTime = 10f;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        //  Debug.Log("Text Component: " + textComponent);
    }

    public IEnumerator ShowAndDestroy()
    {
        yield return new WaitForSeconds(displayTime);
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
