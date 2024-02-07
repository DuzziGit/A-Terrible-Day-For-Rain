using UnityEngine;

public class MoveUpAndDestroy : MonoBehaviour
{
    public float speed = 1.0f;
    public float lifetime = 1.5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}