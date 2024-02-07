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
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillOneLevel < 15)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillOneLevel++;
        }
        else
        {
            button1.interactable = false;
            button1Background.interactable = false;

        }
    }
    public void upgradeSkillTwo()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillTwoLevel < 15)
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillTwoLevel++;
        }
        else
        {
            button2.interactable = false;
            button2Background.interactable = false;

        }

    }
    public void upgradeSkillThree()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillThreeLevel < 15)
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillThreeLevel++;
        }
        else
        {
            button3.interactable = false;
            button3Background.interactable = false;

        }
    }
    public void upgradeSkillUlt()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().ultSkillLevel < 15)
        {

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().ultSkillLevel++;
        }
        else
        {
            buttonUlt.interactable = false;
            buttonUltBackground.interactable = false;

        }
    }
}
