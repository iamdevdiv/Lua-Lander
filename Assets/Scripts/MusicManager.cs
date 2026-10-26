using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {
    public static MusicManager Instance {
        get; private set;
    }

    private AudioSource musicAudioSource;
    private static float musicTime;

    private const int MUSIC_VOLUME_MAX = 10;
    private static int musicVolume = 4;

    public event EventHandler OnMusicVolumeChanged;

    private void Awake() {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }

    private void Update() {
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume() {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetMusicVolume() {
        return musicVolume;
    }

    public float GetMusicVolumeNormalized() {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }
}
