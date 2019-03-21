using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour 
{
#region variables
#endregion
#region initialization
	void Start () 
    {
		
	}
#endregion
#region logic
void Update()
{
	if (Input.GetButtonDown("Restart"))
	{
		GotoMainScene();
	}
	
	if (Input.GetButtonDown("BackToMenu"))
	{
		Application.Quit();
	}
}	
#endregion
#region public interface
public void GotoMainScene()
{
	Helpers.LoadScene("MainScene");
}
#endregion
#region private interface
#endregion
#region events
#endregion
}
