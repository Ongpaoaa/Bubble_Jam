using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Sound Effects List")]
    public List<SoundEffect> soundEffects;

    private Dictionary<string, AudioClip> soundDictionary;
    private AudioSource audioSource;
    private AudioSource loopAudioSource; // Separate AudioSource for looping sounds
    private AudioSource startSource;
    private AudioSource whileSource;
    private AudioSource endSource;

    private bool isSoundSetPlaying = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize
        audioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource = gameObject.AddComponent<AudioSource>();
        loopAudioSource.loop = true; // Ensure the loopAudioSource is set to loop

        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var sound in soundEffects)
        {
            if (!soundDictionary.ContainsKey(sound.name))
            {
                soundDictionary.Add(sound.name, sound.clip);
            }
        }
    }

    public void PlaySound(string soundName, float volume = 1f, float pitch = 1f)
    {
        if (soundDictionary.TryGetValue(soundName, out var clip))
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found in the SoundEffectManager.");
        }
    }

    public void PlaySoundSet(string startSoundName, string whileSoundName, string endSoundName, float volume = 1f, float pitch = 1f)
    {
        if (isSoundSetPlaying)
        {
            Debug.LogWarning("A sound set is already playing. Stop the current one before starting a new set.");
            return;
        }

        isSoundSetPlaying = true;

        // Load clips from the dictionary
        AudioClip startClip = GetSoundClip(startSoundName);
        AudioClip whileClip = GetSoundClip(whileSoundName);
        AudioClip endClip = GetSoundClip(endSoundName);

        if (startClip != null)
        {
            audioSource.clip = startClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();

            // Schedule the "while" sound to play after "start" finishes
            if (whileClip != null)
            {
                StartCoroutine(PlayWhileSoundAfterStart(startClip.length, whileClip, volume, pitch));
            }
        }
    }

    private IEnumerator PlayWhileSoundAfterStart(float delay, AudioClip whileClip, float volume, float pitch)
    {
        yield return new WaitForSeconds(delay);

        loopAudioSource.clip = whileClip;
        loopAudioSource.volume = volume;
        loopAudioSource.pitch = pitch;
        loopAudioSource.Play(); // Start looping the while sound
    }

    public void StopSoundSet(string endSoundName, float volume = 1f, float pitch = 1f)
    {
        if (!isSoundSetPlaying)
        {
            Debug.LogWarning("No sound set is currently playing.");
            return;
        }

        isSoundSetPlaying = false;

        // Stop the "while" sound
        loopAudioSource.Stop();

        // Play the "end" sound
        AudioClip endClip = GetSoundClip(endSoundName);
        if (endClip != null)
        {
            audioSource.clip = endClip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
    }

    private AudioClip GetSoundClip(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out var clip))
        {
            return clip;
        }

        Debug.LogWarning($"Sound '{soundName}' not found in the SoundEffectManager.");
        return null;
    }
}
