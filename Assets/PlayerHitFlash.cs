using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitFlash : MonoBehaviour
{
    private Material flashMaterial;
    private float flashDuration;
    private float flashThreshold; 
    [SerializeField]private float flashRate = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        // Get the material from the Sprite Renderer
        flashMaterial = GetComponent<SpriteRenderer>().material;
    }

    // Call this method to trigger the flash
    public void TriggerFlash(float threshold, float duration)
    {
        flashThreshold = threshold;
        flashDuration = duration;
        StartCoroutine(FlashRoutine());
    }

  private IEnumerator FlashRoutine()
    {
        float elapsedTime = 0f;
        bool isFlashing = false;

        // Continuously flash for the duration
        while (elapsedTime < flashDuration)
        {
            // Toggle the flash
            flashMaterial.SetFloat("_FlashThreshold", isFlashing ? 1f : flashThreshold);

            // Wait for a bit before toggling again
            yield return new WaitForSeconds(flashRate);

            // Increment the elapsed time
            elapsedTime += flashRate;

            // Toggle the flashing state
            isFlashing = !isFlashing;
        }

        // Ensure the flash is turned off after the flashing is done
        flashMaterial.SetFloat("_FlashThreshold", 1f);
    }
}
