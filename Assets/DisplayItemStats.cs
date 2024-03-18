using TMPro;
using UnityEngine;

public class DisplayItemStats : MonoBehaviour
{

    public TMP_Text AttackDamageText;
    public TMP_Text AttackSpeedText;
    public TMP_Text CriticalDamageText;
    public TMP_Text CriticalRateText;
    public TMP_Text TitleText;
    public GameObject ItemPreview; 
    public Sprite BasicWeaponSprite;

public void UpdateStats(WeaponInstance instance)
{
    // Update the UI elements using the instance properties
    // Round to whole number using "N0" format (Number with 0 decimal places)
    AttackDamageText.text = instance.AttackDamage.ToString("N0");
    AttackSpeedText.text = instance.AttackSpeed.ToString("F2"); //
    CriticalDamageText.text = instance.CriticalDamage.ToString("F1");
    CriticalRateText.text = instance.CriticalRate.ToString("F1");
    TitleText.text = instance.Title;
    
}

}

