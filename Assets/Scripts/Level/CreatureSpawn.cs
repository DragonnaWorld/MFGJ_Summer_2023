using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct SpawnInfo
{
    public Bounds bounds;
    public GameObject creature;
}

[Serializable]
public struct WaveInfo
{
    public float timeFromLevelStart;
    public SpawnInfo[] creatureList;
}

public class CreatureSpawn : MonoBehaviour
{
    class WaveComparer : IComparer<WaveInfo>
    {
        public int Compare(WaveInfo x, WaveInfo y)
        {
            if (x.timeFromLevelStart < y.timeFromLevelStart)
                return -1;
            if (x.timeFromLevelStart > y.timeFromLevelStart)
                return 1;
            return 0;
        }
    }
    [SerializeField]
    WaveInfo[] waves;

    float elapsedTime;
    int summonedIndex;

    private void Start()
    {
        elapsedTime = 0F;
        summonedIndex = -1;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        while (summonedIndex < waves.Length - 1 && waves[summonedIndex + 1].timeFromLevelStart <= elapsedTime)
        {
            summonedIndex++;
            SpawnByIndex(summonedIndex);
        }    
    }

    void SpawnByIndex(int index)
    {
        foreach (var creature in waves[index].creatureList)
        {
            Vector3 pos = Vector3.Lerp(creature.bounds.min, creature.bounds.max, UnityEngine.Random.value);
            Instantiate(creature.creature, pos, Quaternion.identity);
        }    
    }    

#if UNITY_EDITOR
    readonly Color[] visualColor = new Color[] { Color.green, Color.blue, Color.red, Color.yellow, Color.cyan, Color.black };
    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < waves.Length; ++i)
        {
            Color color = visualColor[i % visualColor.Length];
            Gizmos.color = color;
            foreach (var spawn in waves[i].creatureList) 
            { 
                Gizmos.DrawWireCube(spawn.bounds.center, spawn.bounds.size);
            }
        }
    }
    private void OnValidate()
    {
        Array.Sort(waves, new WaveComparer());
    }
#endif
}