using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleSystemCallback : MonoBehaviour
{
	#region variables
	public UnityEvent callback;
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
	public void OnParticleSystemStopped()
	{
		callback.Invoke();
	}
	#endregion
}