using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float displayTime = 6f;

    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        //  Debug.Log("Text Component: " + textComponent);
    }


    private void Update() {
        if(gameObject.transform.childCount == 0){
         StartCoroutine(ShowAndDestroy());
        }
    }
    
    private void Start()
    {
        
       // NoChildren();
        //StartCoroutine(ShowAndDestroy());
    }
    private void NoChildren(){
        if(transform.childCount <= 0 ){
            Destroy(gameObject);
        }
    }
    public IEnumerator ShowAndDestroy()
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
