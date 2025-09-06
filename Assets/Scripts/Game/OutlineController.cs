using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private Material material;

    void Awake()
    {
        material = spriteRenderer.material;
    }

    public void EnableOutline()
    {
        if (material != null)
        {
            material.SetFloat("_OutlineAlpha", 1);
        }
    }

    public void DisableOutline()
    {
        if (material != null)
        {
            material.SetFloat("_OutlineAlpha", 0);
        }
    }
}
