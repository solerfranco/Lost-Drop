using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : InteractableElement
{
    protected bool canBeDragged;

    protected override bool canDisableOutline => !isBeingDragged;

    // Virtual property that derived classes can override
    public virtual bool CanBeDragged
    {
        get => canBeDragged;
        set => canBeDragged = value;
    }

    [SerializeField]
    protected bool isBeingDragged;

    public override void OnPointerDown(PointerEventData eventData)
    {
        DOTween.Kill(transform);
        base.OnPointerDown(eventData);
        isBeingDragged = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        isBeingDragged = false;

        if (!isMouseOver)
        {
            DisableOutline();
        }
    }
}
