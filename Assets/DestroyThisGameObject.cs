using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThisGameObject : MonoBehaviour
{
    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
