using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolum;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolum;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int sfxChannelIndex;

    public enum Bgm
    {
        Title, VillageEdenbrook
    }
    public enum Sfx
    { }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolum;
        

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];
        for(int i = 0; i < sfxChannels; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolum;
        }
    }

    public void PlayBgm(Bgm bgm, bool isPlay = true)
    {
        if(isPlay)
        {
            bgmPlayer.clip = bgmClips[(int)bgm];
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }

    }
}
