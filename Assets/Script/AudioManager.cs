using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance;
	
	[SerializeField] private Image muteButtonImage;
	[SerializeField] private Sprite audioOnSprite;
	[SerializeField] private Sprite audioOffSprite;
	
	public const string prefAudioMute = "prefAudioMute";

    [SerializeField] private AudioMixerGroup generalMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] public Sound[] sounds;

    private void Awake() {
    
        Instance = this;
		
		if(PlayerPrefs.HasKey(prefAudioMute)) {
			AudioListener.volume = PlayerPrefs.GetFloat(prefAudioMute);
		}
		
		UpdateMuteButtonImageSprite();

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.isLoop;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.volume = sound.volume;
            
            switch(sound.audioType) {
                case Sound.AudioTypes.SOUNDEFFECT:
                    sound.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
                case Sound.AudioTypes.MUSIC:
                    sound.source.outputAudioMixerGroup = musicMixerGroup;
                    break;
            }

            if(sound.playOnAwake) {
                sound.source.Play();
            }
        }
    }

    public void PlayClipByName(string _clipName) {
        Sound soundToPlay = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        if(soundToPlay != null) {
            soundToPlay.source.Play();
        }
    }

    public void StopClipByName(string _clipName) {
        Sound soundToStop = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        soundToStop.source.Stop();
    }

    public void UpdateMixerVolume() {
        generalMixerGroup.audioMixer.SetFloat("GeneralVolume", MathF.Log10(AudioOptionsManager.generalVolume) * 20);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", MathF.Log10(AudioOptionsManager.musicVolume) * 20);
        soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", MathF.Log10(AudioOptionsManager.soundEffectsVolume) * 20);
    }
	
	public void ToggleMute() {
		if(AudioListener.volume == 0) {
			AudioListener.volume = 1;
		} else {
			AudioListener.volume = 0;
		}
		
		PlayerPrefs.SetFloat(prefAudioMute, AudioListener.volume);		
		
		UpdateMuteButtonImageSprite();
	}
		
	private void UpdateMuteButtonImageSprite() {
		if(AudioListener.volume == 0) {
			muteButtonImage.sprite = audioOffSprite;
		} else {
		    muteButtonImage.sprite = audioOnSprite;
		}
	}
}
