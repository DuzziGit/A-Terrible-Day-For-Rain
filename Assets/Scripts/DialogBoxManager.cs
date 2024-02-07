using UnityEngine;
using UnityEngine.UI;

public class DialogBoxManager : MonoBehaviour
{
    public GameObject textBox;

    public Text dialogText;

    public TextAsset textFile;
    public string[] dialog;

    public int currentLine;
    public int endLine;

    public PlayerMovement player;



    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerMovement>();

        if (textFile != null)
        {
            dialog = (textFile.text.Split('\n'));
        }

        if (endLine == 0)
        {
            endLine = dialog.Length - 1;
        }


    }

    void Update()
    {

        dialogText.text = dialog[currentLine];

        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }

        if (currentLine > endLine)
        {
            textBox.SetActive(false);
        }
    }


}
