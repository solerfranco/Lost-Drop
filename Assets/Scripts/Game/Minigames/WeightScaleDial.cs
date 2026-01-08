using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WeightScaleDial : MonoBehaviour
{
    [SerializeField]
    private Transform needle;

    private float currentWeight;

    public void SetWeight(float weight)
    {
        float needleAngle = Mathf.Lerp(90, -90, weight / 10);

        needle.DOLocalRotate(new Vector3(0, 0, needleAngle), 0.4f).SetEase(Ease.OutBack);

        currentWeight = weight;
    }
}
