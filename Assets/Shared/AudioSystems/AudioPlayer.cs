using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    #region vars
    public bool randomClip = false;
    public AudioClip[] AudioClips;
    public AudioMixerGroup AudioMixerGroup;
    public int channel = 0;
    public bool loop = false;
    public float panning = 0;
    public bool useCustomPitch = false;
    public Vector2 pitch = Vector2.one;
    [SerializeField]
    private bool play2DAtStart = false;

    public AudioSource source { get; private set; }

    int clipIndex = 0;

    #endregion
    #region init
    void Start()
    {
        if (play2DAtStart)
            Play2DSound();
    }
    #endregion
    #region logic
    #endregion
    #region public interface
    public void Play2DSound()
    {
        StopLoopSound();

        source = AudioController.I.PlayAudio2D(GetClip(), AudioMixerGroup, channel);

        SetSourceStats();
    }

    public void Play3DSound()
    {
        StopLoopSound();

        source = AudioController.I.PlayAudio3D(transform.position, GetClip(), AudioMixerGroup, channel);

        SetSourceStats();
    }

    private AudioClip GetClip()
    {
        if (randomClip)
            clipIndex = UnityEngine.Random.Range(0, AudioClips.Length);

        return AudioClips[clipIndex];
    }

    public void StopLoopSound()
    {
        if (source && source.loop)
        {
            source.Stop();
            source = null;
        }
    }
    #endregion
    #region private interface

    private void SetSourceStats()
    {
        if (source)
        {
            source.loop = loop;
            source.panStereo = panning;

            if (useCustomPitch)
                source.pitch = Helpers.Rand(pitch.x, pitch.y);
        }
    }
    #endregion
    #region events
    #endregion
}