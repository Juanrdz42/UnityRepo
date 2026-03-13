using UnityEngine;

public class SFXManager_JuanRdz : MonoBehaviour
{
    public static SFXManager_JuanRdz Instance;

    [Header("References")]
    [SerializeField] private SFXLibrary_JuanRdz sfxLibrary;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource voiceSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        if (ambienceSource == null)
            ambienceSource = gameObject.AddComponent<AudioSource>();

        if (voiceSource == null)
            voiceSource = gameObject.AddComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.spatialBlend = 0f;

        ambienceSource.playOnAwake = false;
        ambienceSource.loop = true;
        ambienceSource.spatialBlend = 0f;

        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
        voiceSource.spatialBlend = 0f;
    }

    public static AudioClip Play(string soundName)
    {
        if (Instance == null || Instance.sfxLibrary == null)
            return null;

        AudioClip clip = Instance.sfxLibrary.GetRandomClip(soundName);

        if (clip == null)
            return null;

        Instance.sfxSource.PlayOneShot(clip);
        return clip;
    }

    public static void PlayAmbience(string soundName)
    {
        if (Instance == null || Instance.sfxLibrary == null)
            return;

        AudioClip clip = Instance.sfxLibrary.GetRandomClip(soundName);

        if (clip == null)
            return;

        Instance.ambienceSource.clip = clip;
        Instance.ambienceSource.Play();
    }

    public static void StopAmbience()
    {
        if (Instance == null || Instance.ambienceSource == null)
            return;

        Instance.ambienceSource.Stop();
    }

    public void PlayVoice(AudioClip clip, float pitch = 1f)
    {
        if (voiceSource == null || clip == null)
            return;

        voiceSource.pitch = pitch;
        voiceSource.PlayOneShot(clip);
    }
}