using FMOD.Studio;
using FMODUnity;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Playlist")] 
    [SerializeField] private List<EventReference> _musicTracks;

    private int _currentIndex = 0;
    private EventInstance _currentMusic;

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

        VolumeManager.instance.LoadVolume();
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
}
