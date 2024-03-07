using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : MonoBehaviour
{
    public static HitEffectPool Instance;

    public GameObject hitEffectPrefab;
    private Queue<GameObject> effectsPool = new Queue<GameObject>();
    public int poolSize = 20;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject effect = Instantiate(
                hitEffectPrefab,
                Vector3.zero,
                Quaternion.identity,
                ContainerManager.Instance.ImpactContainer
            );
            effect.SetActive(false);
            effectsPool.Enqueue(effect);
        }
    }

    public GameObject GetHitEffect()
    {
        if (effectsPool.Count > 0)
        {
            GameObject effect = effectsPool.Dequeue();
            effect.SetActive(true);
            return effect;
        }
        else
        {
            // Optional: Instantiate new ones if pool is empty, can be avoided
            GameObject effect = Instantiate(
                hitEffectPrefab,
                Vector3.zero,
                Quaternion.identity,
                ContainerManager.Instance.ImpactContainer
            );
            return effect;
        }
    }

    public void ReturnHitEffectToPool(GameObject effect)
    {
        effect.SetActive(false);
        effectsPool.Enqueue(effect);
    }
}
