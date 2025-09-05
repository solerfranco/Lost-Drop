using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableElement : InteractableElement
{
    public bool CanBeDragged;

    [SerializeField]
    protected bool isBeingDragged;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        isBeingDragged = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        isBeingDragged = false;
    }
}
