using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Slider slider;

    public void setMaxExp(int playerXp)
    {
        slider.maxValue = playerXp;
        slider.value = 0;
    }

    public void SetExperience(int playerXp)
    {
        slider.value = Mathf.Min(playerXp, slider.maxValue);
    }
}
