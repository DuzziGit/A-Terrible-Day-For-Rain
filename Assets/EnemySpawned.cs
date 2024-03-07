using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawned : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField]
    private float gravScale = 1f; // Example default value

    [SerializeField]
    private float InvicibilityTimer = 1f; // Total time to fade in

    private float currentFadeTime; // Tracks the current fade time

    private void Awake() { }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnInvincibility());
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravScale;
    }

    // Update is called once per frame
    void Update() { }

    private IEnumerator SpawnInvincibility()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(InvicibilityTimer);
        gameObject.layer = LayerMask.NameToLayer("Enemy");
    }
}
