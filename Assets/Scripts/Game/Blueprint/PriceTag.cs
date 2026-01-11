using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

public class PriceTag : SerializedMonoBehaviour
{
    [SerializeField]
    private SpriteRenderer tagRenderer;

    [SerializeField]
    private Dictionary<int, Sprite> tagsByPrice;

    [SerializeField]
    private SpriteRenderer[] ropeStrings;

    [SerializeField]
    private TextMeshProUGUI priceTMP;

    [SerializeField]
    private MMF_Player mMF_Player;

    private int price;

    void Awake()
    {
        mMF_Player.Initialization();
        Reset();
    }

    private void Reset()
    {
        var c = priceTMP.color;
        c.a = 0f;
        priceTMP.color = c;

        tagRenderer.color = Color.clear;
        tagRenderer.transform.eulerAngles = Vector3.forward * -55;
        ropeStrings.ForEach(el =>
        {
            el.color = Color.clear;
        });
    }

    [Button("Show")]
    public void Show()
    {
        var c = priceTMP.color;
        c.a = 1f;
        priceTMP.DOColor(c, 0f).SetEase(Ease.OutQuad);

        tagRenderer.DOColor(Color.white, 0.25f).SetEase(Ease.OutQuad);
        tagRenderer.transform.DORotate(Vector3.forward * 0, 0.25f).SetEase(Ease.OutBack);
        ropeStrings.ForEach(el =>
        {
            el.DOColor(Color.white, 0.25f).SetEase(Ease.OutQuad);
        });
        
    }

    public void Hide()
    {
        var c = priceTMP.color;
        c.a = 0f;
        priceTMP.DOColor(c, 0f).SetEase(Ease.InQuad);

        tagRenderer.DOColor(Color.clear, 0f).SetEase(Ease.InQuad);
        tagRenderer.transform.DORotate(Vector3.forward * -55, 0.25f).SetEase(Ease.InBack);
        ropeStrings.ForEach(el =>
        {
            el.DOColor(Color.clear, 0.25f).SetEase(Ease.InQuad);
        });
    }

    [Button("Upgrade")]
    public void Upgrade(int newPrice)
    {
        price += newPrice;

        mMF_Player.PlayFeedbacks();
        priceTMP.transform.DOKill(true);
        priceTMP.transform.DOScale(Vector3.one * 1.35f, 0.15f).SetEase(Ease.InOutQuad).SetLoops(2, LoopType.Yoyo);
        
        priceTMP.text = $"{price}$";
        tagRenderer.sprite = GetTagForPrice(price);
    }

    private Sprite GetTagForPrice(int price)
    {
        Sprite result = null;
        int bestKey = int.MinValue;

        foreach (var kvp in tagsByPrice)
        {
            if (kvp.Key <= price && kvp.Key > bestKey)
            {
                bestKey = kvp.Key;
                result = kvp.Value;
            }
        }

        return result;
    }
}
