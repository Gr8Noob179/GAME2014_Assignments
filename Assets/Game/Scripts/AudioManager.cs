using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> musicDict = new();
    private Dictionary<string, AudioClip> sfxDict = new();

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (AudioClip clip in musicClips)
        {
            musicDict[clip.name] = clip;
        }

        foreach (AudioClip clip in sfxClips)
        {
            sfxDict[clip.name] = clip;
        }
    }

    public void PlayMusic(string clipName, float fadeDuration = 1f, bool loop = true)
    {
        if (!musicDict.TryGetValue(clipName, out var clip))
        {
            return;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusicRoutine(clip, fadeDuration, loop));
    }

    public void StopMusic(float fadeDuration = 1f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutRoutine(musicSource, fadeDuration));
    }

    private IEnumerator FadeMusicRoutine(AudioClip newClip, float duration, bool loop)
    {
        yield return FadeOutRoutine(musicSource, duration * 0.5f);

        musicSource.clip = newClip;
        musicSource.loop = loop;
        musicSource.Play();

        yield return FadeInRoutine(musicSource, duration * 0.5f);
    }

    public void PlaySFX(string clipName, float volume = 1f)
    {
        if (sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlaySFXAtPosition(string clipName, Vector3 position, float volume = 1f)
    {
        if (!sfxDict.TryGetValue(clipName, out AudioClip clip))
        {
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    private IEnumerator FadeInRoutine(AudioSource source, float duration)
    {
        float startVol = 0f;
        source.volume = 0f;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, 1f, time / duration);
            yield return null;
        }

        source.volume = 1f;
    }

    private IEnumerator FadeOutRoutine(AudioSource source, float duration)
    {
        float startVol = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, 0f, time / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVol;
    }

    public void SetMusicVolume(float volume) => musicSource.volume = volume;
    public void SetSFXVolume(float volume) => sfxSource.volume = volume;
}