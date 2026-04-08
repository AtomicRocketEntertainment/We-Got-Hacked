using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;
    [SerializeField] private Button _toggleSoundBtn;
    [SerializeField] private Slider _volumeSlider;

    private VCA _musicVCA;

    private bool _volumeShow = false;

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
        _toggleSoundBtn.onClick.AddListener(ToggleSlider);
    }

    public void LoadVolume()
    {
        _musicVCA = RuntimeManager.GetVCA("vca:/Music");

        float volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        _volumeSlider.value = volume;

        VolumeChange();
    }

    public void ToggleSlider()
    {
        _volumeShow = !_volumeShow;
        _volumeSlider.gameObject.SetActive(_volumeShow);
    }

    public void VolumeChange()
    {
        float volume = _volumeSlider.value;
        _musicVCA.setVolume(_volumeSlider.value);
        _musicVCA.getVolume(out float volumeVCA);

        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }
}
