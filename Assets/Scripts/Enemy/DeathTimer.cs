using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    [SerializeField]
    Health health;
    [SerializeField]
    float minTimeTilDeath;
    [SerializeField]
    float maxTimeTilDeath;

    float timeTilDeath;
    bool dead;

    public delegate void DespawnBeginHandler(GameObject obj, float timeRemaining);
    public event DespawnBeginHandler OnDespawnBegin;

    private void Start()
    {
        timeTilDeath = Random.Range(minTimeTilDeath, maxTimeTilDeath);
        dead = false;
    }

    void Update()
    {
        if (!dead)
        {
            if (health.Dead)
            {
                dead = true;
                OnDespawnBegin?.Invoke(gameObject, timeTilDeath);
                Invoke(nameof(Despawn), timeTilDeath);
            }    
        }    
    }

    void Despawn()
    {
        Destroy(gameObject);
    }    

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (minTimeTilDeath > maxTimeTilDeath)
            (minTimeTilDeath, maxTimeTilDeath) = (maxTimeTilDeath, minTimeTilDeath);
    }
#endif
}
