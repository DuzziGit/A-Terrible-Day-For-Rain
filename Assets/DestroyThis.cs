using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    private GameObject parent;

    private void Awake()
    {
        InvokeRepeating("DestroyAfterAnimation", 2, 2);
        parent = gameObject.transform.parent.gameObject;
    }

    public void DestroyAfterAnimation()
    {
        // Check if this GameObject has no children
        if (transform.childCount == 0)
        {
            // If it has a parent, destroy the parent
            if (parent != null)
            {
                Destroy(parent);
            }
        }
        else
        {
            // If this GameObject has children, do not destroy the parent
            Debug.Log("GameObject has children, not destroying the parent.");
        }
    }
}
