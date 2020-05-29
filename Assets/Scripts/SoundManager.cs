using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundEffect {
        Coin,
        WoodCrack
    }

    public enum SoundBackground {
        BackgroundMusic,
        HorseRunning
    }

    // Singleton instance.
	private static SoundManager instance = null;
	
	// Initialize the singleton instance.
	private void Awake()
	{
		// If there is not already an instance of SoundManager, set it to this.
		if (instance == null)
		{
			instance = this;
		}
		//If an instance already exists, destroy whatever this object is to enforce the singleton.
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

    public static SoundManager GetInstance() {
        return instance;
    }

    public void PlaySoundEffect(SoundEffect sound) {
        AudioSource.PlayClipAtPoint(GetAudioClip(sound), new Vector3(1f, 0f, 0f));
    }

    public void StopSoundBackground(SoundBackground soundBackground) {
        AudioSource audioSource = GetAudioSource(soundBackground);
        audioSource.Stop();
        audioSource.enabled = false;
    }

    public void PlaySoundBackground(SoundBackground soundBackground) {
		AudioSource audioSource = GetAudioSource(soundBackground);
        audioSource.enabled = true;
        audioSource.Play();
    }

    private AudioClip GetAudioClip(SoundEffect sound) {
        foreach (GameAssets.SoundEffectAudioClip soundEffectAudioClip in GameAssets.GetInstance().soundEffectAudioClipArray)
        {
            if(soundEffectAudioClip.sound == sound) return soundEffectAudioClip.audioClip;
        }

        Debug.Log("Sound: " + sound + "not found!");
        return null;
    }

    private AudioSource GetAudioSource(SoundBackground sound) {
        foreach (GameAssets.SoundBackgroundAudioSource soundBackgroundAudioSource in GameAssets.GetInstance().soundBackgroundAudioSourceArray)
        {
            if(soundBackgroundAudioSource.sound == sound) return soundBackgroundAudioSource.audioSource;
        }

        Debug.Log("Sound: " + sound + "not found!");
        return null;
    }
}
