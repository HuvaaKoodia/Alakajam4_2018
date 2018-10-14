using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAudio : MonoBehaviour
{
    #region variables
    #endregion
    #region initialization
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
    }
    #endregion
    #region logic
    #endregion
    #region public interface
    #endregion
    #region private interface
    #endregion
    #region events
    private void OnValueChanged(bool value)
    {
        GUIAudioController.I.PlayToggleClick();
    }
    #endregion
}
