using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBasedOnPlatform : MonoBehaviour
{
	#region variables
	public bool onMobile = true, onWeb = false;
	#endregion
	#region initialization
	void Awake()
	{
#if UNITY_ANDROID
		if (onMobile)
			gameObject.SetActive(false);
#elif UNITY_WEBGL
		if (onWeb)
			gameObject.SetActive(false);
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