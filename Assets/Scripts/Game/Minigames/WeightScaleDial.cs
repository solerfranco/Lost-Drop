using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WeightScaleDial : MonoBehaviour
{
    [SerializeField]
    private Transform needle;

    [SerializeField]
    private Transform indicator;

    private float currentWeight;

    public void SetWeight(float weight)
    {
        float needleAngle = Mathf.Lerp(82, -82, weight / 10);

        needle.DOLocalRotate(new Vector3(0, 0, needleAngle), 0.4f).SetEase(Ease.OutBack);

        currentWeight = weight;
    }

    public void SetIdealWeight(float weight)
    {
        float indicatorAngle = Mathf.Lerp(-12, -172, weight / 10);

        indicator.DOLocalRotate(new Vector3(0, 0, indicatorAngle), 0.4f).SetEase(Ease.OutBack);
    }
}
