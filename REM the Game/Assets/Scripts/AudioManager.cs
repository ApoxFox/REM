using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sfx;
    public AudioSource[] bgm;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            PlayBGM(5);
        }
    }

    //sound effects are set in an array and this is use to choose which number to play
    public void PlaySFX(int soundToPlay)
    {
        if(soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        if(!bgm[musicToPlay].isPlaying)
        {
            StopMusic();

            if(musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
        
}
