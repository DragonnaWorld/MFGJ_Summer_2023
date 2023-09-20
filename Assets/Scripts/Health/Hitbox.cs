using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// "Hitbox" and "Hurtbox" layers should only collide with each other
/// </summary>
public class Hitbox : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    HitboxManager manager;

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer(Internal.LayerConfig.Hitbox);
    }

    private void OnTriggerEnter(Collider hurtbox)
    {
        manager.RegisterHit(hurtbox, damage);
    }
}
