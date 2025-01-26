using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
    }

    public static SoundManager Instance;

    public float fadeTime = .5f;
    public List<SoundEntry> soundEntries;

    private AudioSource _audioSource;
    private float _defaultVolume;


    private void Awake()
    {
        // Singleton pattern setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("No AudioSource component found on the SoundManager GameObject!");
        }
        else
        {
            _defaultVolume = _audioSource.volume;
        }
    }

    private static string playingClip = null;

    /// <summary>
    /// Plays (loops) the AudioClip associated with the given key.
    /// </summary>
    /// <param name="clipKey">The name/key assigned to the clip.</param>
    public static void Play(string clipKey)
    {
        if (Instance == null)
        {
            Debug.LogError("SoundManager instance not found in scene.");
            return;
        }

        if (Instance._audioSource == null)
        {
            Debug.LogError("SoundManager AudioSource is missing.");
            return;
        }

        if (clipKey == playingClip)
        {
            //Don't play the same clip twice
            return;
        }

        var clip = Instance.soundEntries.FirstOrDefault(e => e.name == clipKey);
        if (clip != null)
        {
            playingClip = clipKey;
            Instance.StartCoroutine(Instance.FadeAndPlay(clip.clip));
        }
        else
        {
            Debug.LogWarning($"SoundManager: No clip found for key '{clipKey}'");
        }
    }

    /// <summary>
    /// Main routine that fades out any currently playing track, switches to the new clip, then fades in.
    /// </summary>
    private IEnumerator FadeAndPlay(AudioClip clip)
    {
        // If something is already playing, fade it out first
        if (_audioSource.isPlaying && _audioSource.clip != null)
        {
            yield return StartCoroutine(FadeOut(fadeTime));
        }

        // Set the new clip (and enable looping, if desired)
        _audioSource.clip = clip;
        _audioSource.loop = true;

        // Fade in the new clip
        yield return StartCoroutine(FadeIn(fadeTime));
    }

    /// <summary>
    /// Fade out the currently playing audio over 'duration' seconds and stop.
    /// </summary>
    private IEnumerator FadeOut(float duration)
    {
        float startVolume = _audioSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null; // wait for next frame
        }

        _audioSource.Stop();
        // Restore volume to starting level (just in case)
        _audioSource.volume = startVolume;
    }

    /// <summary>
    /// Fade in a newly assigned clip from 0 volume to the default volume over 'duration' seconds.
    /// </summary>
    private IEnumerator FadeIn(float duration)
    {
        _audioSource.volume = 0f;
        _audioSource.Play();

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0f, _defaultVolume, time / duration);
            yield return null; // wait for next frame
        }

        _audioSource.volume = _defaultVolume;
    }
}
