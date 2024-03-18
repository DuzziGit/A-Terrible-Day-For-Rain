using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, float ShakeForce)
    {
        impulseSource.GenerateImpulseWithForce(ShakeForce);
    }
      public void CameraShakeVec3(CinemachineImpulseSource impulseSource, Vector3 ShakeForce)
    {
        impulseSource.GenerateImpulse(ShakeForce);
    }


}
