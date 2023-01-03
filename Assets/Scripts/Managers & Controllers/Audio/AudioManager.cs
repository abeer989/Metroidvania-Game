using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Title("Audio Clips")]
    [SerializeField] List<AudioClip> musicClips;
    [SerializeField] List<AudioClip> SFXClips;

    [Title("Audio Sources")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource SFXAudioSource;

    int currentLevelMusicIndex = -1;

    private void Awake()
    {
        // Singleton:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    public void PlayMusic(int index)
    {
        if(index != currentLevelMusicIndex)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = musicClips[index];
            musicAudioSource.loop = true;
            musicAudioSource.Play();
            currentLevelMusicIndex = index;
        }
    }

    public void PlaySFX(SFXData sfxData)
    {
        if (sfxData.adjust)
            SFXAudioSource.pitch = Random.Range(1, 1.2f);

        if (SFXAudioSource.isPlaying)
            SFXAudioSource.Stop();

        SFXAudioSource.PlayOneShot(SFXClips[sfxData.SFXIndex]);
    }
}
