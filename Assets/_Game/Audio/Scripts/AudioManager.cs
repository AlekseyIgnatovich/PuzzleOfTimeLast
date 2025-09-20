using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] AudioData data;
    [SerializeField] AudioSource trackPlayer;

    int playSet = -1;

    private void OnEnable() {
        data.OnTrackChange += PlayerTrack;
        data.OnTrackVolumeChange += SetVolume;
        data.OnTrackMuteChange += SetMute;
    }
    private void OnDisable() {
        data.OnTrackChange -= PlayerTrack;
        data.OnTrackVolumeChange -= SetVolume;
        data.OnTrackMuteChange -= SetMute;
    }

    private void Start() {
        SetMute();
        SetVolume();
    }

    public void PlayerTrack() {
        if ((data.track == null) || (trackPlayer.isPlaying && (data.playSet == playSet))) {
            return;
        }

        playSet = data.playSet;
        trackPlayer.clip = data.track;
        trackPlayer.Play();
    }

    void SetMute() {
        trackPlayer.mute = data.trackMute;
        //if (trackPlayer.mute) { trackPlayer.Stop(); }
    }
    void SetVolume() {
        trackPlayer.volume = data.trackVolume;
    }
}
