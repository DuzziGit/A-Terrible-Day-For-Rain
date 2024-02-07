using UnityEngine;

public class ImpactAnim : MonoBehaviour
{
    public GameObject hitEffect; // Reference to the first hit effect prefab
    public GameObject hitEffect2; // Reference to the second hit effect prefab
    public Animator animation1;
    public Animator animation2;
    private float destroyDelay = 0.5f; // Delay before destroying the projectile

  private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Projectile"))
    {
        Vector2 collisionPoint = collision.transform.position;
        Quaternion enemyRotation = collision.transform.rotation;

        GameObject effect1 = Instantiate(hitEffect, collisionPoint, enemyRotation);
        GameObject effect2 = Instantiate(hitEffect2, collisionPoint, enemyRotation);

        Animator effect1Animator = effect1.GetComponent<Animator>();
        Animator effect2Animator = effect2.GetComponent<Animator>();

        if (effect1Animator != null)
        {
            effect1Animator.SetBool("PlayAnimation", true);
        }
        if (effect2Animator != null)
        {
            effect2Animator.SetBool("PlayAnimation", true);
        }

        Destroy(effect1, destroyDelay);
        Destroy(effect2, destroyDelay);
    }
}

}
