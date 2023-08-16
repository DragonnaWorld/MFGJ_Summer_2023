using System;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    [SerializeField]
    Array2D<GameObject> hitboxes;
    [SerializeField]
    Hurtbox[] ExcludeHurtboxes;

    readonly HashSet<int> exclusion = new();
    readonly HashSet<int> hurtboxesThisAttack = new();

    static readonly IDGenerator IDManager = new(500);

    public int HitID { get; private set; }

    private void Start()
    {
        foreach (var noHit in ExcludeHurtboxes)
            exclusion.Add(noHit.ID);
        foreach (var attackSet in hitboxes.array)
            foreach (var hitbox in attackSet.array)
                hitbox.SetActive(false);
    }

    public void StartHitboxes(int id)
    {
        foreach (var hitbox in hitboxes[id])
            hitbox.SetActive(true);
        HitID = IDManager.Generate();
    }

    public void DisableHitboxes(int id)
    {
        foreach (var hitbox in hitboxes[id])
            hitbox.SetActive(false);
        IDManager.Retrieve(HitID);
        HitID = -1;
        hurtboxesThisAttack.Clear();
    }

    public void RegisterHit(Collider hurtbox, float damage)
    {
        Hurtbox data = hurtbox.GetComponent<Hurtbox>();
        if (exclusion.Contains(data.ID))
            return;
        bool hurtboxRegistered = hurtboxesThisAttack.Contains(data.ID);
        if (hurtboxRegistered)
            return;

        data.Health.DecreaseHealth(damage);
        hurtboxesThisAttack.Add(data.ID);
    }
} 