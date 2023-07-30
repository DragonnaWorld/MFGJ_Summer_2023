using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer[] sprites;

    void Start()
    {
        GenerateDarkColor();
    }

    [ContextMenu("Generate Color")]
    void GenerateDarkColor()
    {
        Color color = new(Random.Range(0, 0.5F), Random.Range(0, 0.5F), Random.Range(0, 0.5F));
        foreach (var sprite in sprites) 
            sprite.color = color;
    }
}
