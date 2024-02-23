using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [SerializeField] private GameObject shuriken;
    // Start is called before the first frame update
    [SerializeField]
    private Vector3[] offsets = new Vector3[]
           {
        new Vector3(0, 0.2f, 0), // Top shuriken
        new Vector3(0, 0, 0),    // Middle shuriken
        new Vector3(0, -0.2f, 0) // Bottom shuriken
           };
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float throwForce = 1f;
    [SerializeField]
    private float[] throwAngles; // Angles for each shuriken
    private string uniqueAttackId;
    private string UniqueAttackId
    {
        get { return uniqueAttackId; }
    }

    private void Awake()
    {
        uniqueAttackId = HitManager.GenerateSkillActivationGuid();
        StartCoroutine(TripleThrow());
        Invoke("DestroyPrefab", lifetime);
    }
    private IEnumerator TripleThrow()
    {
        bool isFacingRight = GameController.instance.playerMovement.facingRight;
        for (int i = 0; i < offsets.Length; i++)
        {
            var offsetsDirection = offsets[i];

            float angle = throwAngles.Length > i ? throwAngles[i] : 0;

            // Adjust the angle based on the facing direction
            if (!isFacingRight)
            {
                angle = -angle; // Invert the angle when facing left
                offsetsDirection = -offsetsDirection;
            }

            GameObject shurikenInstance = Instantiate(shuriken, transform.position + offsetsDirection, transform.rotation, gameObject.transform);
            shurikenInstance.GetComponent<Projectile>().UniqueAttackId = UniqueAttackId;

            // Calculate the base throw direction with the adjusted angle
            Vector3 baseThrowDirection = Quaternion.Euler(0, 0, angle) * transform.right;

            ShurikenMovement shurikenMovement = shurikenInstance.GetComponent<ShurikenMovement>();
            if (shurikenMovement != null)
            {
                // Pass transform.position as the start position
                shurikenMovement.SetMovement(baseThrowDirection.normalized, throwForce, transform.position);
            }
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }
    private void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
