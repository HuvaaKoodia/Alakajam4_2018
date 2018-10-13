using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour
{
#region variables
	public static MainController I;
	
	public LevelGenerator generator;
#endregion
#region initialization
	private void Awake()
	{
		I = this;
        
	}
	
	IEnumerator Start()
	{
		while (!LevelDatabase.I.loadingDone)
			yield return null;
			
		generator.Generate();
		
	}
#endregion
#region logic
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}
