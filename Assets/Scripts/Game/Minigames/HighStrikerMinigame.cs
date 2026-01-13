using System;
using DG.Tweening;
using UnityEngine;

public class HighStrikerMinigame : MonoBehaviour
{
    [SerializeField]
    private Transform metalPuk;

    [SerializeField]
    private float maxYPosition;

    private float initialYPosition;

    void Start()
    {
        initialYPosition = metalPuk.localPosition.y;
        MovementLoop();
    }

    private void MovementLoop()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(metalPuk.DOLocalMoveY(maxYPosition, 0.5f).SetEase(Ease.InQuart));
        sequence.Append(metalPuk.DOLocalMoveY(initialYPosition, 0.5f).SetEase(Ease.OutQuart));
        sequence.SetLoops(-1);
    }
}
