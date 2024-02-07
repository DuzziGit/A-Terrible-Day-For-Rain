using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public double maximumXp;
    public int playerXp;
    public int playerLevel;
    public int playerHealth;
    public int money;
    private bool didLoad = false;
    /// public ExperienceBar experienceBar;

    public string textValue = "LEVEL UP";
    public Text textElement;

    // used to set the players position when loading the game
    private Vector3 position;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

        money = 1111;

        DontDestroyOnLoad(this.gameObject);
        /*
                if (!didLoad)
                {

                    // load game data when player loads, we can do this wherever though once the menus are implemented.
                    GameData data = SaveState.LoadGameData();
                    // check if we have something returned from LoadGameData(), if we dont thats a big problem.
                    if (data != null)
                    {
                        // get values from the read saved data and set those to the player, we're ignoring levels for now since those aren't done at this time, easy to add after the fact though.
                        playerLevel = data.playerLevel;
                        playerHealth = data.playerHealth;
                        playerXp = data.playerExperience;

                        // get the saved position of the player, set that position on load
                        position = new Vector3(data.position[0], data.position[1], data.position[2]);


                        if (data.sceneIndex != SceneManager.GetActiveScene().buildIndex)
                        {
                            SceneManager.LoadScene(data.sceneIndex, LoadSceneMode.Single);

                            // could probably just use this.position but I'm too lazy to find out
                            GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = position;
                        }

                        didLoad = true;


                    }
                    else // something went wrong when loading the data, use whatever the defaults that were already here for the player stats.
                    {
                        maximumXp = 20;
                        playerLevel = 1;
                        playerHealth = 100;
                    }
                }*/
    }

    // Update is called once per frame
    void Update()
    {
        playerXp = ExperienceController.experience;
        //Debug.Log("current xp" + ExperienceController.experience);
        //Debug.Log("player level" + playerLevel);
        // LevelUp();


    }
    //     void OnTriggerEnter2D(Collider2D collision) {
    //  if (collision.gameObject.tag == "Experience")
    //         {
    //             LevelUp();
    //         }
    //         }

    //  public void LevelUp() {
    //      if (playerXp >= maximumXp) {
    //         playerLevel++;
    //          playerXp = 0;
    //          maximumXp = (playerLevel * 100 * 1.2); 


    //         //textElement.text = textValue;
    //     }
    // }
}



