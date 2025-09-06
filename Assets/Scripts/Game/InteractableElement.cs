using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    private Material material;

    protected virtual bool canDisableOutline => true;

    protected bool isMouseOver;

    [FoldoutGroup("Pointer Events")]
    [SerializeField]
    protected UnityEvent onPointerDown, onPointerUp, onPointerEnter, onPointerExit;


    protected virtual void Awake()
    {
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    protected void EnableOutline()
    {
        if (material != null)
        {
            material.SetFloat("_OutlineAlpha", 1);
        }
    }

    protected void DisableOutline()
    {
        if (material != null && canDisableOutline)
        {
            material.SetFloat("_OutlineAlpha", 0);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        onPointerEnter?.Invoke();
        EnableOutline();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        onPointerExit?.Invoke();
        DisableOutline();
    }

}
