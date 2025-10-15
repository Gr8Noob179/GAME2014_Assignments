using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        float savedMusicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float savedSfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.value = savedMusicVol;
        sfxSlider.value = savedSfxVol;

        ApplyMusicVolume(savedMusicVol);
        ApplySfxVolume(savedSfxVol);

        musicSlider.onValueChanged.AddListener(ApplyMusicVolume);
        sfxSlider.onValueChanged.AddListener(ApplySfxVolume);
    }

    private void ApplyMusicVolume(float value)
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }

        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void ApplySfxVolume(float value)
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }

        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}