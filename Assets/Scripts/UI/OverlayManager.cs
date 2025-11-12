using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    public static OverlayManager Instance;

    [SerializeField]
    private Image overlay;

    [SerializeField]
    private Color overlayColor;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void FadeToBlack()
    {
        overlay.enabled = true;

        DOTween.Kill(overlay);
        overlay.DOColor(overlayColor, 0.15f).SetEase(Ease.InOutSine);
    }

    public void Clear()
    {
        DOTween.Kill(overlay);
        overlay.DOColor(Color.clear, 0.15f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            overlay.enabled = false;
        });
    }
}
