using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundTypeID
{
    _None,
    Effect,
    UI,
    Music,
    Voiceover,
    _Amount,
}

public class AudioController : MonoBehaviour
{
    [System.Serializable]
    public class Channel
    {
        public string name;
        public AudioSource source;
        public int sourceAmount = 20;
        public float repeatTimeout = -1;
        public SoundTypeID type;

        [HideInInspector]
        public float repeatTimer = 0;

        public bool CheckTimeout()
        {
            if (repeatTimeout == -1)return false;

            if (repeatTimer > Time.time)return true;

            repeatTimer = Time.time + repeatTimeout;
            return false;
        }
    }

    #region vars
    public static AudioController I;

    public Channel[] channels;

    public AudioMixer mixer;
    public bool fullVolume { get; private set; }

    AudioSource[][] audioSources;
    int[] currentSourceIndex;
    List<AudioSource> sources = new List<AudioSource>();

    float[] volumes, settingsVolumes;
    // public float masterVolume { get {}}
    public float musicVolume { get { return volumes[(int)SoundTypeID.Music]; } }
    // public float effectsVolume { get; private set; }
    // public float uiVolume { get; private set; }

    #endregion
    #region init
    private void Awake()
    {
        if (I != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        audioSources = new AudioSource[channels.Length][];
        currentSourceIndex = new int[channels.Length];
        volumes = new float[(int)SoundTypeID._Amount];
        settingsVolumes = new float[(int)SoundTypeID._Amount];

        for (int c = 0; c < channels.Length; c++)
        {
            audioSources[c] = new AudioSource[channels[c].sourceAmount];
            for (int a = 0; a < channels[c].sourceAmount; a++)
            {
                var source = Instantiate(channels[c].source)as AudioSource;
                audioSources[c][a] = source;
                sources.Add(source);

                DontDestroyOnLoad(source.gameObject);
                source.transform.parent = transform;
                source.gameObject.SetActive(false);
            }

            if (channels[c].source.transform.parent == transform)channels[c].source.gameObject.SetActive(false);
        }
    }

    #endregion
    #region logic
    #endregion
    #region public interface

    public AudioSource PlayAudio2D(AudioClip clip, AudioMixerGroup mixerGroup, int channel = 0)
    {
        return GetSource(clip, mixerGroup, channel);
    }

    public AudioSource PlayAudio3D(Vector3 position, AudioClip clip, AudioMixerGroup mixerGroup, int channel = 0)
    {
        var source = GetSource(clip, mixerGroup, channel);

        if (source == null)return null;

        source.spatialBlend = 0f;
        source.transform.position = position;
        return source;
    }

    public void SetMasterVolume(float volume)
    {
        volumes[0] = volume;
        settingsVolumes[0] = volume;

        SetMixerVolume("MasterVolume", volume);

// #if UNITY_WEBGL
//         if (webGLmusicSource != null)
//             webGLmusicSource.volume = musicVolume * masterVolume;
// #endif
    }

    public void SetMusicVolume(float volume, bool settingsValue = true)
    {
        if (settingsValue)
            settingsVolumes[(int)SoundTypeID.Music] = volume;
        volumes[(int)SoundTypeID.Music] = volume;

// #if UNITY_WEBGL
//         if (webGLmusicSource != null)
//             webGLmusicSource.volume = musicVolume * masterVolume;
// #else
        SetMixerVolume("MusicVolume", volume);
// #endif
    }

    public void SetEffectsVolume(float volume)
    {
        volumes[(int)SoundTypeID.Effect] = volume;
        settingsVolumes[(int)SoundTypeID.Effect] = volume;

        SetMixerVolume("EffectsVolume", volume);
    }

    public void SetUIVolume(float volume)
    {
        volumes[(int)SoundTypeID.UI] = volume;
        settingsVolumes[(int)SoundTypeID.UI] = volume;

        SetMixerVolume("UIVolume", volume);
    }

    internal void SetVOVolume(float volume)
    {
        volumes[(int)SoundTypeID.Voiceover] = volume;
        settingsVolumes[(int)SoundTypeID.Voiceover] = volume;

        SetMixerVolume("VOVolume", volume);
    }


    private void SetMixerVolume(string parameter, float volume)
    {
        if (volume == 0)
            mixer.SetFloat(parameter, -80);
        else
            mixer.SetFloat(parameter, -20 + 20 * volume);
    }

    public void SetMixerVolumeNormalized(string parameter, float volume)
    {
        if (volume == 0)
            mixer.SetFloat(parameter, -80);
        else
            mixer.SetFloat(parameter, -40 + 40 * volume);
    }

    public void StopSoundsAll()
    {
        for (int i = 1; i < sources.Count; i++)
            sources[i].Stop();
    }

    public void StopSounds(int channel)
    {
        foreach (var source in audioSources[channel])
            source.Stop();
    }

    public void StopSounds(SoundTypeID type)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            if (channels[i].type == type)
                StopSounds(i);
        }
    }

    #endregion
    #region private interface

    private AudioSource GetSource(AudioClip clip, AudioMixerGroup mixerGroup, int channel)
    {
        var channelData = channels[channel];
        if (channelData.CheckTimeout())return null;
        //if (channelData.type != SoundTypeID.Music && settingsVolumes[(int)channelData.type] == 0)return null;

        var source = audioSources[channel][currentSourceIndex[channel]];
        source.gameObject.SetActive(true);
        currentSourceIndex[channel]++;
        if (currentSourceIndex[channel] == channelData.sourceAmount)currentSourceIndex[channel] = 0;

        source.pitch = 1f;
        source.spatialBlend = 0f;
        source.outputAudioMixerGroup = mixerGroup;
        source.clip = clip;
        source.Play();
        return source;
    }

    #endregion
    #region events
    #endregion
}