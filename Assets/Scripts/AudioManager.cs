using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] List<AudioSource> music;
    [SerializeField] List<AudioSource> SFX;

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

    public void PlayMainMenuMusic()
    {
        music.ForEach(a => a.Stop());
        music[0].Play(); // main menu music on 0th index
    }    
    
    public void PlayLevelMusic()
    {
        if (!music[1].isPlaying)
        {
            music.ForEach(a => a.Stop());
            music[1].Play(); // level music on 1st index 
        }
    }    
    
    public void PlayBossMusic()
    {
        music.ForEach(a => a.Stop());
        music[2].Play(); // boss music on 2nd index
    }
}
