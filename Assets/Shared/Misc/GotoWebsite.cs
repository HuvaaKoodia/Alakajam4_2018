using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoWebsite : MonoBehaviour 
{
#region variables
public string website = "https://www.example.com";
#endregion
#region initialization
#endregion
#region logic
#endregion
#region public interface
public void Goto()
{
	Application.OpenURL(website);
}
#endregion
#region private interface
#endregion
#region events
#endregion
}
