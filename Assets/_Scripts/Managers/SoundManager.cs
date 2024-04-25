using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    public AudioSource _sfxSource;

    [SerializeField]
    private AudioClip[] _audClips;

    public enum Sounds 
    {
        PlaceTower = 0,
        PistolFire = 1,
        ButtonClick = 2,
        EnemyDeath = 3,
        
    }

    private void Start()
    {
        GameObject musicPlayer = new GameObject("MusicPlayer");
        _musicSource = musicPlayer.AddComponent<AudioSource>();
        _musicSource.loop = true;
        GameObject sfxPlayer = new GameObject("SFXPlayer");
        _sfxSource = sfxPlayer.AddComponent<AudioSource>();
    }

    public void PlayOneShot(Sounds sound) 
    {
        _sfxSource.PlayOneShot(GetAudioClip(sound));
    }

    private AudioClip GetAudioClip(Sounds sound) 
    {
        return _audClips[(int)sound];
    }

    public void StartMusic() 
    {
        _musicSource.Play();
    }
    
    public void StopMusic() 
    {
        _musicSource.Stop();
    }

    public void ChangeMusic(AudioClip musicClip)
    {
        _musicSource.clip = musicClip;
    }
}
