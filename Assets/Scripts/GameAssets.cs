using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    public Transform ground;
    public Transform borderLeft;
    public Transform borderRight;
    public Transform pfFence;
    public Transform pfCoin;

    public SoundEffectAudioClip[] soundEffectAudioClipArray;
    public SoundBackgroundAudioSource[] soundBackgroundAudioSourceArray;

    [Serializable]
    public class SoundEffectAudioClip {
        public SoundManager.SoundEffect sound;
        public AudioClip audioClip;
    }

    [Serializable]
    public class SoundBackgroundAudioSource {
        public SoundManager.SoundBackground sound;
        public AudioSource audioSource;
    }
}
