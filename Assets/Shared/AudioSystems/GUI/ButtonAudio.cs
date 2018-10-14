using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    #region variables
    public AudioPlayer defaultOverride;
    #endregion
    #region initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    #endregion
    #region logic
    #endregion
    #region public interface
    #endregion
    #region private interface
    #endregion
    #region events
    private void OnButtonClick()
    {
        if (defaultOverride)
            defaultOverride.Play2DSound();
        else
            GUIAudioController.I.PlayButtonClick();
    }
    #endregion
}