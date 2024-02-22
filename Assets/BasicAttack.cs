using System;
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

    private IEnumerator TripleThrow()
    {

        for (int i = 0; i < offsets.Length; i++)
        {
            Instantiate(shuriken, gameObject.transform.localPosition + offsets[i], gameObject.transform.rotation);

            // Wait for two fixed updates after each instantiation
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }
    // Update is called once per frame
    private void Awake()
    {
        StartCoroutine(TripleThrow());
        //Invoke("DestroyPrefab", lifetime);

    }

    private void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
