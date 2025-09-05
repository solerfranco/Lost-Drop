using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField]
    private RectTransform optionsScreen;

    [SerializeField]
    private Slider sfxSlider, musicSlider;

    void Start()
    {
        sfxSlider.value = MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Sfx, false);
        musicSlider.value = MMSoundManager.Instance.GetTrackVolume(MMSoundManager.MMSoundManagerTracks.Music, false);
    }

    public void Open()
    {
        OverlayManager.Instance.FadeToBlack();

        optionsScreen.gameObject.SetActive(true);
        optionsScreen.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack, 3);
    }

    public void Close()
    {
        OverlayManager.Instance.Clear();

        optionsScreen.DOAnchorPosY(1200, 0.3f).SetEase(Ease.InBack, 3).OnComplete(() =>
        {
            optionsScreen.gameObject.SetActive(false);
        });
    }

    public void SetSfxVolume(float volume)
    {
        MMSoundManager.Instance.SetVolumeSfx(volume);
    }

    public void SetMusicVolume(float volume)
    {
        MMSoundManager.Instance.SetVolumeMusic(volume);
    }
}
