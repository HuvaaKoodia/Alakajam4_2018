using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    #region variables
    public static LoadingCanvas I;
    public GameObject panel;
    public Text text;
    #endregion
    #region initialization
    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        GameObject.DontDestroyOnLoad(gameObject);
    }
    #endregion
    #region logic
    #endregion
    #region public interface
    public void Show()
    {
        Show("Loading...");
    }
    
    public void Show(string text)
    {
        panel.SetActive(true);
        this.text.text = text;
    }
    
    public void Hide()
    {
        panel.SetActive(false);
    }
    #endregion
    #region private interface
    #endregion
    #region events
    #endregion
}