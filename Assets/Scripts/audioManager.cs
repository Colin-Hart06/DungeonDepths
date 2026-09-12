using System.Collections;
using UnityEngine.Audio;
using System;
using UnityEngine;
public class audioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static audioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found or not initialized!");
            return;
        }
        s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public bool IsPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

    // NEW - fade out over a given duration, then stop
    public void FadeOut(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found or not initialized!");
            return;
        }
        StartCoroutine(FadeOutRoutine(s, duration));
    }

    private IEnumerator FadeOutRoutine(Sound s, float duration)
    {
        float startVolume = s.source.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            s.source.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        s.source.volume = 0f;
        s.source.Stop();
        s.source.volume = startVolume; // reset for next time it's played
    }
}