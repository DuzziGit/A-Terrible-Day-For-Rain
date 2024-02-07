using UnityEngine;
using UnityEngine.UI;

public class PhealthBar : MonoBehaviour
{
    public Slider slider;
    public Canvas healthBarCanvas; // Add this line to get a reference to the Canvas

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0; // Set the minimum value to 0
        slider.value = maxHealth;
        healthBarCanvas.enabled = false; // Set the canvas to be initially hidden
    }

    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}