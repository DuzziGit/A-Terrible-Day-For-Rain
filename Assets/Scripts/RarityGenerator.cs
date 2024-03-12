using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RarityGenerator
{
    public static int WeightedProb(int[] tiers, float[] weights)
    {
        float totalWeight = 0;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }
        float p = Random.Range(0, totalWeight);
        float runningTotal = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            runningTotal += weights[i];
            if (p < runningTotal) return tiers[i];
        }
        return -1; // Consider returning a default or error value if the weights don't add up properly
    }
}