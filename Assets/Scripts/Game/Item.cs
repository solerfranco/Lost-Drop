using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Item : DraggableElement
{
    #region Data

    private MaterialType materialType;
    public MaterialType MaterialType => materialType;

    private int weight = 0;
    public int Weight => weight;
    private MaterialRarity materialRarity;

    #endregion

    private Vector3 velocity;
    private Vector3 initialScale;

    [SerializeField]
    private Transform sprite;

    [SerializeField]
    private Transform tooltip;

    [SerializeField]
    private TextMeshProUGUI weightTMP;

    [SerializeField]
    private TextMeshProUGUI itemTMP;
    [SerializeField]
    private Image itemTypeImage;
    [SerializeField]
    private Image tooltipBackground;

    // [SerializeField]
    // private SerializedDictionary<MaterialType, MaterialData> materialsData;

    // [SerializeField]
    // private SerializedDictionary<MaterialRarity, Sprite> tooltipBackgroundRarities;

    protected override void Awake()
    {
        base.Awake();
        initialScale = sprite.localScale;
        // weightTMP.text = weight.ToString();
        // itemTMP.text = materialsData[materialType].Description;
        // itemTypeImage.sprite = materialsData[materialType].Sprite;
        // tooltipBackground.sprite = tooltipBackgroundRarities[materialRarity];
        RandomizeRotation();
    }

    [Serializable]
    public class MaterialData
    {
        public Sprite Sprite;
        public string Description;
    }

    public void Initialize(ItemDataSO itemData)
    {
        materialType = itemData.materialType;
        materialRarity = itemData.materialRarity;
        spriteRenderer.sprite = itemData.sprite;
        weight = itemData.weight;
    }

    public void RandomizeRotation()
    {
        sprite.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        DOTween.Kill(transform);
        velocity = Vector3.zero;

        sprite.DOScale(initialScale * 1.3f, 0.5f).SetEase(Ease.OutBack);
        transform.SetParent(null);
        transform.position = new(transform.position.x, transform.position.y, -5);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (transform.position.y > 0)
        {
            DOTween.Kill(transform, true);
            transform.DOMove(new Vector3(transform.position.x, -2f, transform.position.z), 0.75f).SetEase(Ease.OutBounce);
            transform.GetChild(0).DOScale(Vector3.one, 0.75f).SetEase(Ease.OutQuad);
            return;
        }

        // Optional inertia throw
        sprite.DOScale(initialScale, 1f).SetEase(Ease.OutBounce);

        //Set z position to 0
        Vector3 targetPos = transform.position;
        targetPos.z = 0;
        transform.position = targetPos;

        //Calculate mouse position
        Vector2 mouseScreenPos = Pointer.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        //Move the item to mouse last position to make the dragging feel better
        transform.DOMove(worldPos, 0.25f).SetEase(Ease.OutQuad);

        VerifyItemPlacement(worldPos);
        
    }

    public void VerifyItemPlacement(Vector3 pointerWorldPosition)
    {
        // Raycast all colliders at mouse position to find conveyor belt or Weapon 
        RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(pointerWorldPosition, Vector2.zero);

        bool droppedOnWrongFragment = false;
        foreach (var hit in raycastHit2Ds)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<ConveyorBelt>(out var conveyor))
                {
                    conveyor.PlaceItem(transform);
                }
                if (hit.collider.TryGetComponent<WeaponFragment>(out var weaponPiece))
                {
                    if (weaponPiece.CanPlaceItem(this))
                    {
                        transform.SetParent(weaponPiece.transform);
                        transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutQuad);
                        transform.DOLocalMove(Vector3.zero + Vector3.forward * weaponPiece.transform.position.z, 0.25f).SetEase(Ease.OutQuad);
                        sprite.DOLocalRotate(Vector3.forward * weaponPiece.transform.rotation.eulerAngles.z, 0.25f).SetEase(Ease.OutQuad);
                        droppedOnWrongFragment = false;
                        break;
                    }
                    else
                    {
                        droppedOnWrongFragment = true;
                    }
                }
            }
        }
        if (droppedOnWrongFragment)
        {
            transform.DOMove(transform.position + Vector3.left * UnityEngine.Random.Range(2f, 4f), 0.25f).SetEase(Ease.OutQuad);
        }
    }

    public void DisableCollider()
    {
        Collider2D col = GetComponentInChildren<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    public void EnableCollider()
    {
        Collider2D col = GetComponentInChildren<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        DOTween.Kill(tooltip, true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        DOTween.Kill(tooltip);
    }

    private Transform darkHoleTransform;

    public void EnterDarkHole(Transform darkHole)
    {
        darkHoleTransform = darkHole;
        GetComponentInChildren<Collider2D>().enabled = false;
        DOTween.Kill(transform);
        transform.DOScale(Vector3.zero, 0.8f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void Update()
    {
        // if (darkHoleTransform == null) return;

        // transform.position = Vector3.MoveTowards(transform.position, darkHoleTransform.position, 5 * Time.deltaTime);
        // transform.Rotate(Vector3.back * 200 * Time.deltaTime);

        if (isBeingDragged)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            worldPos.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, 0.1f);
        }
    }
}
