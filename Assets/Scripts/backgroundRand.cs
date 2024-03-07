using UnityEngine;
using UnityEngine.UI;

public class backgroundRand : MonoBehaviour
{
    public Image myImage; // The Image component on the current GameObject
    public Sprite[] imageArray; // The array of images to choose from

    private void Start()
    {
        // Choose a random image from the array
        int randomIndex = Random.Range(0, imageArray.Length);
        Sprite randomImage = imageArray[randomIndex];

        // Set the current GameObject's image to the chosen image
        myImage.sprite = randomImage;
    }
}
