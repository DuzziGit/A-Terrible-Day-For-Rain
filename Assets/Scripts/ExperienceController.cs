using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public static int experience;
    public GameObject expPixel;

    private void Start()
    {
        // ignore exp items' own hitbox
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), expPixel.GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }


}
