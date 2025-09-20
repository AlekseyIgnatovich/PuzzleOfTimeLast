using UnityEngine;
using System;

[CreateAssetMenu(menuName ="My File/System/Audio Data")]
public class AudioData : ScriptableObject {

    [SerializeField] MusicTracks trackList;
    [HideInInspector] public AudioClip track;
    [HideInInspector] public int playSet;
    [Space]
    public float masterVolume;

    public bool sfxMute;
    public float sfxVolume;

    public bool trackMute;
    public float trackVolume;

    public event Action OnTrackChange;
    public event Action OnTrackMuteChange;
    public event Action OnTrackVolumeChange;

    private void OnEnable() {
        AudioListener.volume = masterVolume;
    }

    public void PlaySFX(AudioClip _clip) {
        if (!sfxMute) {
            AudioSource.PlayClipAtPoint(_clip, -Vector3.forward * 10, sfxVolume);
        }
    }
    public void PlayMenuTrack() {
        track = trackList.titleTrack[UnityEngine.Random.Range(0, trackList.titleTrack.Length)];
        playSet = 0;
        OnTrackChange?.Invoke();
    }
    public void PlayBattleTrack() {
        track = trackList.battleTrack[UnityEngine.Random.Range(0, trackList.battleTrack.Length)];
        playSet = 1;
        OnTrackChange?.Invoke();
    }

    public void MasterVolume(float val) {
        masterVolume = val;
        AudioListener.volume = val;
    }
    public void TrackVolume(float val) {
        trackVolume = val;
        OnTrackVolumeChange?.Invoke();
    }
    public void TrackToggle() {
        trackMute = !trackMute;
        OnTrackMuteChange?.Invoke();
        if (!trackMute) { PlayMenuTrack(); }
    }
    public void SFXVolume(float val) {
        sfxVolume = val;
    }
    public void SFXToggle() {
        sfxMute = !sfxMute;
    }

}
