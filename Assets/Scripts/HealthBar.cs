using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Canvas healthBarCanvas;

    private float lastHealthChangeTime;
    private int lastHealth;
    private Coroutine healthCheckCoroutine;

    private void Awake()
    {
        // Initialize lastHealth with an impossible value to ensure it's set on the first update
        lastHealth = -1;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0;
        slider.value = maxHealth;
        healthBarCanvas.enabled = false;
        lastHealthChangeTime = Time.time;
    }

    public void SetHealth(int currentHealth)
    {
        if (currentHealth != lastHealth)
        {
            slider.value = currentHealth;
            lastHealth = currentHealth;
            lastHealthChangeTime = Time.time;

            // Enable the health bar canvas when health is updated
            if (currentHealth < slider.maxValue)
            {
                healthBarCanvas.enabled = true;
            }

            // Restart the coroutine to check for health stability
            if (healthCheckCoroutine != null)
            {
                StopCoroutine(healthCheckCoroutine);
            }
            healthCheckCoroutine = StartCoroutine(CheckHealthStability());
        }

        // Disable the health bar if health is below 0
        if (currentHealth <= 0)
        {
            healthBarCanvas.enabled = false;
        }
    }

    private IEnumerator CheckHealthStability()
    {
        yield return new WaitForSeconds(4); // Wait for 4 seconds

        // If the health hasn't changed in the last 4 seconds, disable the health bar
        if (Time.time - lastHealthChangeTime >= 4)
        {
            healthBarCanvas.enabled = false;
        }
    }
}
