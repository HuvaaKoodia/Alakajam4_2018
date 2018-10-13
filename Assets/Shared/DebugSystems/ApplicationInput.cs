using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationInput : MonoBehaviour
{
	#region variables
	public bool onlyInEditor = false;
	#endregion
	#region initialization
	#endregion
	#region logic
	void Update()
	{
		#if !UNITY_EDITOR
			if (onlyInEditor) return;
		#endif
		
		if (Input.GetKeyDown(KeyCode.R))
			ReloadScene();
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
	#endregion
	#region public interface
	public void ReloadScene()
	{
		Helpers.ReloadScene();
	}
	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}