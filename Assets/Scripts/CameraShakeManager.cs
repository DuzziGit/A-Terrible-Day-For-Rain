using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [SerializeField] private float globalShakeFource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeFource);
    }
}
