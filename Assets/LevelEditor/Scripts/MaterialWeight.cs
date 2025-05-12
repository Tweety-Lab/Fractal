using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialWeight
{
    [Tooltip("The material to use.")]
    public Material Material;

    [Tooltip("Chance of using this material. 1 = Even chance, 0 = No chance.")]
    [UnityEngine.Range(0f, 1f)] // Enforce a range of 0 - 1
    public float Weight = 1f;

    /// <summary>
    /// Get a random material based on weight.
    /// </summary>
    public static Material GetRandomMaterial(List<MaterialWeight> materials)
    {
        // Get total randomness sum
        float totalRandomness = 0f;
        foreach (var materialWeight in materials)
        {
            totalRandomness += materialWeight.Weight;
        }

        // Get a random value between 0 and total randomness
        float randomValue = Random.Range(0f, totalRandomness);

        // Loop through materials and pick one based on weight
        float cumulativeWeight = 0f;
        foreach (var materialWeight in materials)
        {
            cumulativeWeight += materialWeight.Weight;

            if (randomValue <= cumulativeWeight)
            {
                return materialWeight.Material;
            }
        }

        // Default return if something goes wrong
        return materials[0].Material;
    }
}