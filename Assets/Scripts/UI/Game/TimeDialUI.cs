using UnityEngine;

public class TimeDialUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform dialHand;

    [SerializeField]
    private float startAngle, endAngle;

    void Update()
    {
        UpdateClockHand();
    }
    
    private void UpdateClockHand()
    {
        float angle = Mathf.Lerp(startAngle, endAngle, TimeManager.Instance.RemainingFraction);
        dialHand.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

}
