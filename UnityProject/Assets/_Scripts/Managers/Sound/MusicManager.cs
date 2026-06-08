using FMOD.Studio;
using FMODUnity;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private bool _playOnStart = true;
    [Header("Playlist")] 
    [SerializeField] private List<EventReference> _musicTracks;
    [Header("Sound Effects")]
    [SerializeField] private List<SoundEffects> _soundEffects;

    private int _currentIndex = 0;
    private EventInstance _currentMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _currentIndex = 0;
        if (_musicTracks.Count == 0) return;

        StartCoroutine(LoadMusicBank());
    }

    private void OnDestroy()
    {
        _currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _currentMusic.release();
    }

    private void Update()
    {
        if (!_currentMusic.isValid()) return;

        _currentMusic.getPlaybackState(out var state);
        if(state == PLAYBACK_STATE.STOPPED) NextTrack();
    }
    

    private IEnumerator LoadMusicBank()
    {
        RuntimeManager.LoadBank("Master");
        RuntimeManager.LoadBank("Music");

        while (!RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        RuntimeManager.CoreSystem.mixerSuspend();
        RuntimeManager.CoreSystem.mixerResume();

        yield return new WaitForSeconds(0.1f);
        
        EventManager.MusicBanksLoaded();
        if (_playOnStart)
            PlayCurrentTrack();
    }

    private void PlayCurrentTrack()
    {
        _currentMusic = RuntimeManager.CreateInstance(_musicTracks[_currentIndex]);
        _currentMusic.start();
    }

    private void NextTrack()
    {
        _currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _currentMusic.release();

        _currentIndex++;
        if(_currentIndex >= _musicTracks.Count) _currentIndex = 0;

        PlayCurrentTrack();
    }

    private void PlaySFX(EventReference sfx)
    {
        var instance = RuntimeManager.CreateInstance(sfx);

        instance.start();
        instance.release();
    }

    public void StartTrack()
    {
        PlayCurrentTrack();
    }

    public void PlayKeySfx(string key)
    {
        var sfx = _soundEffects.Find(se => se.key == key);
        if (sfx != null)
        {
            PlaySFX(sfx.sfx);
        }
    }
}

[System.Serializable]
public class SoundEffects
{
    public string key;
    public EventReference sfx;
}
