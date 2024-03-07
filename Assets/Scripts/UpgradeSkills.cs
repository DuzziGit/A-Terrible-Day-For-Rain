using UnityEngine;
using UnityEngine.UI;
public class UpgradeSkills : MonoBehaviour
{


    public Button button1;
    public Button button1Background;
    public Button button2;
    public Button button2Background;
    public Button button3;
    public Button button3Background;
    public Button buttonUlt;
    public Button buttonUltBackground;



    public void Start()
    {

    }
    public void upgradeSkillOne()
    {
        if (GameManager.Instance.playerMovement.skillOneLevel < 15)
        {
            GameManager.Instance.playerMovement.skillOneLevel++;
        }
        else
        {
            button1.interactable = false;
            button1Background.interactable = false;

        }
    }
    public void upgradeSkillTwo()
    {
        if (GameManager.Instance.playerMovement.skillTwoLevel < 15)
        {

            GameManager.Instance.playerMovement.skillTwoLevel++;
        }
        else
        {
            button2.interactable = false;
            button2Background.interactable = false;

        }

    }
    public void upgradeSkillThree()
    {
        if (GameManager.Instance.playerMovement.skillThreeLevel < 15)
        {

            GameManager.Instance.playerMovement.skillThreeLevel++;
        }
        else
        {
            button3.interactable = false;
            button3Background.interactable = false;

        }
    }
    public void upgradeSkillUlt()
    {
        if (GameManager.Instance.playerMovement.ultSkillLevel < 15)
        {

            GameManager.Instance.playerMovement.ultSkillLevel++;
        }
        else
        {
            buttonUlt.interactable = false;
            buttonUltBackground.interactable = false;

        }
    }
}
