using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchUpwards : MonoBehaviour
{
private Rigidbody2D rb;
[SerializeField] private float launchForce;
    void Start()
    {
    rb = GetComponent<Rigidbody2D>();
    rb.AddForce(Vector2.up * launchForce,ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
