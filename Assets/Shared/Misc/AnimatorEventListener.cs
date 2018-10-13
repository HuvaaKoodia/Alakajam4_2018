using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEventListener : MonoBehaviour
{
    #region variables
    public UnityEvent onEvent1;
    #endregion
    #region initialization
    #endregion
    #region logic
    #endregion
    #region public interface
    #endregion
    #region private interface
    #endregion
    #region events
    public void CallEvent1()
    {
        onEvent1.Invoke();
    }
    #endregion
}
