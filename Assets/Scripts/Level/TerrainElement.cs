using System.Collections.Generic;
using UnityEngine;

public class TerrainElement : MonoBehaviour
{
    [SerializeField]
    Dictionary<string, float> Penalty;

    public float GetPelnatyPointFor(string CreatureType)
    {
        if (Penalty.ContainsKey(CreatureType))
            return Penalty[CreatureType];   
        return 0F;
    }
}