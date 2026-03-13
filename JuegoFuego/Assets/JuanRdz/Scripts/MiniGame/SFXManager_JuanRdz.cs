using UnityEngine;
using System.Collections.Generic;

public class SFXManager_JuanRdz : MonoBehaviour
{
    public static SFXManager_JuanRdz Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public Sound[] sounds;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();

    private AudioSource sfxSource;
    private AudioSource ambienceSource;
    private AudioSource voiceSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        ambienceSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambienceSource.loop = true;

        foreach (Sound s in sounds)
        {
            if (!soundDictionary.ContainsKey(s.name) && s.clip != null)
                soundDictionary.Add(s.name, s.clip);
        }
    }

    public static AudioClip Play(string soundName)
    {
        if (Instance == null)
            return null;

        if (!Instance.soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return null;
        }

        AudioClip clip = Instance.soundDictionary[soundName];
        Instance.sfxSource.PlayOneShot(clip);
        return clip;
    }

    public static void PlayAmbience(string soundName)
    {
        if (Instance == null)
            return;

        if (!Instance.soundDictionary.ContainsKey(soundName))
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        Instance.ambienceSource.clip = Instance.soundDictionary[soundName];
        Instance.ambienceSource.Play();
    }

    public static void StopAmbience()
    {
        if (Instance == null)
            return;

        Instance.ambienceSource.Stop();
    }

    public void PlayVoice(AudioClip clip, float pitch = 1f)
    {
        if (clip == null || voiceSource == null)
            return;

        voiceSource.pitch = pitch;
        voiceSource.PlayOneShot(clip);
    }
}



