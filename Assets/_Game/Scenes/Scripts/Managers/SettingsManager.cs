using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Manager {

    [SerializeField] AudioData audioData;
    [Space]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider trackVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;
    [SerializeField] TogglerButton trackToggler;
    [SerializeField] TogglerButton sfxToggler;

    private void OnEnable() {
        trackToggler.OnToggleChange += SetTrackMute;
        sfxToggler.OnToggleChange += SetSFXMute;
    }
    private void OnDisable() {
        trackToggler.OnToggleChange -= SetTrackMute;
        sfxToggler.OnToggleChange -= SetSFXMute;
    }

    private void Start() {
        masterVolumeSlider.value = audioData.masterVolume;
        trackVolumeSlider.value = audioData.trackVolume;
        SFXVolumeSlider.value = audioData.sfxVolume;
        trackToggler.SetToggle(!audioData.trackMute, true);
        sfxToggler.SetToggle(!audioData.sfxMute, true);
    }

    public void SetMasterVolume() {
        audioData.MasterVolume(masterVolumeSlider.value);
    }
    public void SetTrackVolume() {
        audioData.TrackVolume(trackVolumeSlider.value);
    }
    public void SetSFXVolume() {
        audioData.SFXVolume(SFXVolumeSlider.value);
    }
    public void SetTrackMute() {
        audioData.TrackToggle();
        trackToggler.SetToggle(!audioData.trackMute, true);
    }
    public void SetSFXMute() {
        audioData.SFXToggle();
        sfxToggler.SetToggle(!audioData.sfxMute, true);
    }


    public void OpenURL(string _url) {
        Application.OpenURL(_url);
        print($"Opened [{_url}] URL");
    }
}
