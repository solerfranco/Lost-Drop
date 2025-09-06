using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class WeaponWeight
{
    public int MinWeight;
    public int MaxWeight;
}

public class Customer : MonoBehaviour
{
    [SerializeField]
    private float baseValue = 0.25f;
    public SerializedDictionary<Weapon, WeaponWeight> weightByWeapon;

    [SerializeField]
    private SerializedDictionary<int, Weapon[]> weaponsByDay;

    [SerializeField]
    private SerializedDictionary<int, float> patienceByDay;

    [SerializeField]
    private Transform weaponHoldingTransform;

    [SerializeField]
    private Sprite[] sprites;

    public SerializedDictionary<Weapon, Sprite> weaponSprites;


    private Weapon weaponType;

    [SerializeField]
    private SpriteRenderer spriteRenderer, lightSpriteRenderer;

    [SerializeField]
    private Transform dialogBubble;

    // public CustomerQueue queue;

    private bool isOnCounter;

    [SerializeField]
    private TextMeshProUGUI weightTMP;

    private int weight;

    [SerializeField]
    private Image itemSprite;

    [SerializeField]
    private Transform weightPerformanceIndicator;

    [SerializeField]
    private Transform rightWeightIndicator, wrongWeightIndicator;
    
    [SerializeField]
    private Transform weaponPerformanceIndicator;
    
    [SerializeField]
    private Transform rightWeaponIndicator, wrongWeaponIndicator;

    [SerializeField]
    private AudioClip doorChimeBells;

    private Vector3 initialPosition;

    private float currentPatience;

    // private DeliveryUI deliveryUI;

    public void Initialize(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    void Start()
    {
        initialPosition = transform.position;

        // AudioManager.Instance.PlaySFX(doorChimeBells, 1, UnityEngine.Random.Range(0.9f, 1.1f));

        // currentPatience = patienceByDay[TimeManager.Instance.CurrentDay];

        RandomizeItem();

        // deliveryUI = DeliveriesSystem.Instance.CreateDeliveryWidget(currentPatience, weaponType, weight);
    }

    void Update()
    {
        // currentPatience -= Time.deltaTime;
        // if (currentPatience <= 0)
        // {
        //     GetComponent<Collider2D>().enabled = false;
        //     MoveToPosition(Vector3.right * 15);
        //     // queue.customers[0] = null;
        //     // queue.MoveTheQueueForward();
        //     // deliveryUI.DeleteWidget();
        //     currentPatience = 1000;
        //     // WinScreen.Instance.WrongDeliveries++;
        //     Destroy(gameObject, 5f);
        // }

    }

    // Public accessor for the currently displayed sprite
    public Sprite CurrentSprite => spriteRenderer != null ? spriteRenderer.sprite : null;

    public void TweenSpriteSize(Vector3 targetScale)
    {
        spriteRenderer.transform.DOScale(targetScale, 0.3f).SetEase(Ease.OutQuad);
    }

    public void MoveToPosition(Vector2 position, bool goingToCounter, float startingDelay = 0)
    {
        
        if (goingToCounter)
        {
            isOnCounter = goingToCounter;
            dialogBubble.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }

        DOTween.Kill($"Breathing{GetInstanceID()}");
        Sequence sequence = DOTween.Sequence();

        sequence.AppendInterval(startingDelay);
        sequence.Append(transform.DOLocalMoveX(position.x, baseValue * 8).SetEase(Ease.InOutSine));
        sequence.Join(transform.DOLocalMoveY(transform.localPosition.y + 0.25f, baseValue).SetEase(Ease.InOutSine).SetLoops(8, LoopType.Yoyo));
        sequence.Join(transform.DOScaleX(0.95f, baseValue).SetEase(Ease.InOutSine).SetLoops(8, LoopType.Yoyo));
        sequence.Join(transform.DOScaleY(1.05f, baseValue).SetEase(Ease.InOutSine).SetLoops(8, LoopType.Yoyo));

        sequence.onComplete += () =>
        {
            Sequence breathingSequence = DOTween.Sequence();
            breathingSequence.AppendInterval(UnityEngine.Random.Range(0f, 0.5f));
            breathingSequence.Append(transform.DOMoveY(initialPosition.y + 0.15f, baseValue * 8).SetEase(Ease.InOutSine));
            breathingSequence.Append(transform.DOMoveY(initialPosition.y, baseValue * 8).SetEase(Ease.InOutSine));
            breathingSequence.SetLoops(-1, LoopType.Yoyo);
            breathingSequence.SetId($"Breathing{GetInstanceID()}");
        };
    }

    private void RandomizeItem()
    {
        // WeaponType[] availableWeapons = weaponsByDay[TimeManager.Instance.CurrentDay];

        // weaponType = availableWeapons[UnityEngine.Random.Range(0, availableWeapons.Length)];

        // itemSprite.sprite = weaponSprites[weaponType];

        // weight = UnityEngine.Random.Range(weightByWeapon[weaponType].MinWeight, weightByWeapon[weaponType].MaxWeight + 1);

        // weightTMP.text = weight.ToString();
    }

    public bool ReceiveItem(WeaponBlueprint weaponBlueprint)
    {
        bool isRightWeapon = weaponBlueprint.Weapon == weaponType;
        bool isRightweight = weaponBlueprint.Weight == weight;

        float goldMultiplier = 1;

        if (isOnCounter)
        {
            weaponBlueprint.transform.SetParent(transform);
            weaponBlueprint.transform.position = weaponHoldingTransform.position;

            weaponBlueprint.GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(sprite =>
            {
                sprite.sortingLayerID = spriteRenderer.sortingLayerID;
                sprite.sortingOrder = spriteRenderer.sortingOrder + 1;
            });

            weaponPerformanceIndicator.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            weightPerformanceIndicator.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            // rightWeaponIndicator.gameObject.SetActive(isRightWeapon);
            // wrongWeaponIndicator.gameObject.SetActive(!isRightWeapon);

            // rightWeightIndicator.gameObject.SetActive(isRightweight);
            // wrongWeightIndicator.gameObject.SetActive(!isRightweight);

            if (!isRightWeapon)
            {
                goldMultiplier -= 0.5f;
            }

            if (!isRightweight)
            {
                goldMultiplier -= 0.5f;
            }

            MoveToPosition(Vector3.right * 15, false, 1);
            
            // queue.MoveTheQueueForward();
            // deliveryUI.DeleteWidget();
            GoldManager.Instance.AddGold((int)(50 * goldMultiplier));

            if (!isRightWeapon && !isRightweight)
            {
                // WinScreen.Instance.WrongDeliveries++;
            }
            else
            {
                // WinScreen.Instance.rightDeliveries++;
            }

            currentPatience = 1000;
            Destroy(gameObject, 5f);
        }

        return isOnCounter;
    }
}
