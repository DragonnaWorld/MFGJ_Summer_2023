using UnityEngine;
using UnityEngine.UI;

public class HealthRenderer : MonoBehaviour
{
    [SerializeField]
    Image foregroundBar;
    [SerializeField]
    Image middleGroundBar;
    [SerializeField]
    Health healthData;
    [SerializeField]
    [Range(1F, 3F)]
    float reduceFactor;

    private void Update()
    {
        foregroundBar.fillAmount = healthData.Normalized;
        middleGroundBar.fillAmount = Mathf.Lerp(middleGroundBar.fillAmount, healthData.Normalized, reduceFactor * Time.deltaTime);
    }
}