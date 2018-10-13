using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotate : MonoBehaviour 
{
#region variables
	public float speed = 1f;
	public Space space;
	public Vector3 axis;
	public bool randomAxis = false;
	public bool unscaledTime = false;
#endregion
#region initialization
	void Start()
	{
		if(randomAxis)
			axis = Random.onUnitSphere;
	}
#endregion
#region logic
	private void Update () 
	{
		if (unscaledTime)
			transform.Rotate(axis, Time.unscaledDeltaTime * speed, space);
		else 
			transform.Rotate(axis, Time.deltaTime * speed, space);
	}
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}
