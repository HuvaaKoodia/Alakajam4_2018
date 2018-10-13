using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoScene : MonoBehaviour 
{
#region variables
#endregion
#region initialization
#endregion
#region logic
#endregion
#region public interface
public void Goto(string sceneName)
{
	SceneManager.LoadScene(sceneName);
}
#endregion
#region private interface
#endregion
#region events
#endregion
}
