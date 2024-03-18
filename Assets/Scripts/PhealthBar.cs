using UnityEngine;
using UnityEngine.UI;

public class PhealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0; // Set the minimum value to 0
        slider.value = maxHealth;
    }

    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }
}
