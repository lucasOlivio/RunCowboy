using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound {
        Coin
    }

    public static void PlaySound(Sound sound) {
        AudioSource.PlayClipAtPoint(GetAudioClip(sound), new Vector3(1f, 0f, 0f));
    }

    private static AudioClip GetAudioClip(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound) return soundAudioClip.audioClip;
        }

        Debug.Log("Sound: " + sound + "not found!");
        return null;
    }
}
