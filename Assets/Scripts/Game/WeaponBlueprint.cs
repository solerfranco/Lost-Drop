using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WeaponBlueprint : DraggableElement
{
    private Vector3 velocity;

    [SerializeField]
    private WeaponRecipeSO weaponRecipe;

    public Weapon Weapon => weaponRecipe.Weapon;

    private int currentHits = 0;
    private int weight;
    public int Weight => weight;

    [SerializeField]
    private WeaponFragment[] weaponFragments;

    [SerializeField]
    private SpriteRenderer weaponOutline, hammerIcon;

    [SerializeField]
    private ParticleSystem dustParticleSystem;

    void Start()
    {
        foreach (WeaponFragment fragment in weaponFragments)
        {
            fragment.OnPlacedChange += CheckIfReadyToBuild;
        }
    }

    void OnDisable()
    {
        foreach (WeaponFragment fragment in weaponFragments)
        {
            fragment.OnPlacedChange -= CheckIfReadyToBuild;
        }
    }

    private void CheckIfReadyToBuild()
    {
        hammerIcon.gameObject.SetActive(ArePiecesPlaced);
    }

    public bool Hit()
    {
        //Check if all pieces are placed
        if (!ArePiecesPlaced) return false;

        // ScreenShake.Instance.TriggerShake(0.5f);

        currentHits++;
        dustParticleSystem.Play();
        if (currentHits >= weaponRecipe.HitsNeeded)
        {
            //Weapon assembly is complete
            Debug.Log("Weapon Assembly Complete!");

            // Disable all child colliders

            weaponOutline.enabled = false;
            spriteRenderer.transform.DOScale(Vector3.zero, 0.3f);

            foreach (var collider in GetComponentsInChildren<Collider2D>().Skip(1))
            {
                collider.enabled = false;
            }

            weight = GetComponentsInChildren<Item>().Sum(item => item.Weight);
        }

        return currentHits >= weaponRecipe.HitsNeeded;
    }

    public bool ArePiecesPlaced => weaponFragments.All(p => p.IsPlaced);

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (currentHits < weaponRecipe.HitsNeeded)
        {
            if (transform.position.y > 0)
            {
                transform.DOMoveY(-2.5f, 1f).SetEase(Ease.OutBounce);
            }
            return;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(worldPos, Vector2.zero);

        foreach (var hit in raycastHit2Ds)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<Customer>(out var customer))
                {
                    customer.ReceiveItem(this);
                    enabled = false;
                    return;
                }
            }
        }

        if (transform.position.y > 0)
        {
            transform.DOMoveY(-2.5f, 1f).SetEase(Ease.OutBounce);
        }
        else
        {
            transform.DOMove(worldPos, 0.25f).SetEase(Ease.OutQuad);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    void Update()
    {
        if (isBeingDragged)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            worldPos.z = transform.position.z;

            transform.position = Vector3.SmoothDamp(transform.position, worldPos, ref velocity, 0.1f);
        }
    }
}

