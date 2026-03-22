using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbientManager : MonoBehaviour
{
    [Header("Playlist")]
    [SerializeField] private EventReference _ambienceTrack;

    private EventInstance _ambienceInstance;

    private void Start()
    {
        if (_ambienceTrack.IsNull) return;

        RuntimeManager.LoadBank("Ambient");
        PlayAmbianceTrack();
    }

    void PlayAmbianceTrack()
    {
        _ambienceInstance = RuntimeManager.CreateInstance(_ambienceTrack);
        _ambienceInstance.start();
    }
}
