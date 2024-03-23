using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitFlash : MonoBehaviour
{
    private Material flashMaterial;
    private float flashDuration;
    private float flashThreshold;

    [SerializeField]
    private float flashRate = 0.2f;

    [SerializeField]
    private bool shouldFlash;

    void Start()
    {
        flashMaterial = GetComponent<SpriteRenderer>().material;
    }

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

        if (shouldFlash)
        {
            while (elapsedTime < flashDuration)
            {
                flashMaterial.SetFloat("_FlashThreshold", isFlashing ? 1f : flashThreshold);
                yield return new WaitForSeconds(flashRate);
                elapsedTime += flashRate;
                isFlashing = !isFlashing;
            }
        }
        else
        {
            flashMaterial.SetFloat("_FlashThreshold", flashThreshold);
            yield return new WaitForSeconds(flashRate);
        }

        // Ensure the flash is turned off after the flashing is done
        flashMaterial.SetFloat("_FlashThreshold", 1f);
    }
}
