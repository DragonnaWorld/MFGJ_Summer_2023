using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField]
    [field: Range(0F, 100F)]
    public float MaxHealth { get; private set; }

    public bool Dead { get { return currentValue <= 0; } }
    public float Normalized
    {
        get
        {
            return currentValue / MaxHealth;
        }
        set
        {
            value = Mathf.Clamp01(value);
            currentValue = MaxHealth * value;
        }
    }

    float currentValue;

    private void Start()
    {
        currentValue = MaxHealth;
    }

    public void DecreaseHealth(float value)
    {
        currentValue -= value;
        currentValue = Mathf.Clamp(currentValue, 0, MaxHealth);
    }

    public void IncreaseHealth(float value)
    {
        currentValue += value;
        currentValue = Mathf.Clamp(currentValue, 0, MaxHealth);
    }

    
}
