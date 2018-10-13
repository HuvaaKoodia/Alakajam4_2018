using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionNumberText : MonoBehaviour
{
	#region variables
	public Text text;
	#endregion
	#region initialization
	void Start()
	{
		text.text = "V." + Application.version;

#if DEMO
		text.text += " DEMO";
#endif
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