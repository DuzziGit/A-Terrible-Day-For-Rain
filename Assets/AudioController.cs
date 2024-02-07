using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    public AudioSource audioSource;
    public AudioClip monsterHurtClip;
    public AudioClip levelUp;
    public AudioClip jumpSound;
    private void Awake()
    {
        //  DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMonsterHurtSound()
    {
        audioSource.PlayOneShot(monsterHurtClip);

    }
    public void PlayLevelUpSound()
    {
        audioSource.PlayOneShot(levelUp);

    }
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);

    }
}
