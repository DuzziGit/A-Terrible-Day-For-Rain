using UnityEngine;

public class musicClass : MonoBehaviour
{
    public AudioSource backgroundMusic;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.Play();


    }


    public void PlayMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            return;
        }

        backgroundMusic.Play();
    }


    public void StopMusic()
    {
        backgroundMusic.Stop();
    }
}
