using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance {
        get; private set;
    }

    private AudioSource audioSource;
    private const int SOUND_VOLUME_MAX = 10;
    private static int soundVolume = 6;

    [SerializeField] private AudioClip fuelPickupAudioClip;
    [SerializeField] private AudioClip coinPickupAudioClip;
    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip landingSuccesAudioClip;

    public event EventHandler OnSoundVolumeChange;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lander_onCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        switch (e.landingType) {
            case Lander.LandingType.Success:
                audioSource.PlayOneShot(landingSuccesAudioClip);
                break;
            default:
                audioSource.PlayOneShot(crashAudioClip);
                break;
        }
    }

    private void Lander_onCoinPickup(object sender, System.EventArgs e) {
        Debug.Log("Playing coin pickup audio clip");
        audioSource.PlayOneShot(coinPickupAudioClip);
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e) {
        audioSource.PlayOneShot(fuelPickupAudioClip);
    }

    public void ChangeSoundVolume() {
        soundVolume = (soundVolume + 1) % SOUND_VOLUME_MAX;
        audioSource.volume = GetSoundVolumeNormalized();
        OnSoundVolumeChange?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume() { 
        return soundVolume; 
    }

    public float GetSoundVolumeNormalized() {
        return ((float)soundVolume) / SOUND_VOLUME_MAX;
    }
}
