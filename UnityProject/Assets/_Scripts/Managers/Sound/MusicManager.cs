using FMOD.Studio;
using FMODUnity;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Playlist")] 
    [SerializeField] private List<EventReference> _musicTracks;
    [SerializeField] private EventReference _ambienceTrack;

    private int _currentIndex = 0;
    private EventInstance _currentMusic;
    private EventInstance _ambienceInstance;

    private void Start()
    {
        _currentIndex = 0;
        if(_musicTracks.Count > 0) PlayCurrentTrack();

        if(!_ambienceTrack.IsNull) PlayAmbianceTrack();
    }

    private void OnDestroy()
    {
        _ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _ambienceInstance.release();
    }

    private void Update()
    {
        if (!_currentMusic.isValid()) return;

        _currentMusic.getPlaybackState(out var state);

        if(state == PLAYBACK_STATE.STOPPED) NextTrack();
    }

    void PlayAmbianceTrack()
    {
        _ambienceInstance = RuntimeManager.CreateInstance(_ambienceTrack);
        _ambienceInstance.start();
    }

    void PlayCurrentTrack()
    {
        _currentMusic = RuntimeManager.CreateInstance(_musicTracks[_currentIndex]);
        _currentMusic.start();
    }

    void NextTrack()
    {
        _currentMusic.release();

        _currentIndex++;
        if(_currentIndex >= _musicTracks.Count) _currentIndex = 0;

        PlayCurrentTrack();
    }
}
