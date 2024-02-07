using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    public static int experience;
    public GameObject expPixel;


    void Start()
    {
        // ignore exp items' own hitbox
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), expPixel.GetComponent<Collider2D>());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }


}
