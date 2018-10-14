using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIAudioController : MonoBehaviour
{
    #region variables
    public static GUIAudioController I;
    public AudioPlayer buttonClick, toggleClick;
    #endregion
    #region initialization
    void Awake()
    {
        I = this;
    }
    #endregion
    #region logic
    #endregion
    #region public interface
    public void PlayButtonClick()
    {
        buttonClick.Play2DSound();
    }

    public void PlayToggleClick()
    {
        toggleClick.Play2DSound();
    }
    #endregion
    #region private interface
    #endregion
    #region events
    #endregion
}
