using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToParent : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = Vector3.zero;
    }
}
