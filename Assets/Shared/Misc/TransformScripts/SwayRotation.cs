using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayRotation : MonoBehaviour 
{
#region variables
public float speed = 1f;
public float maxAngle = 45f;
public Vector3 axis;
float angle;
#endregion
#region initialization
#endregion
#region logic
	void Update () 
    {
		angle += Time.deltaTime * speed;
		transform.localRotation = Quaternion.Euler(axis * Mathf.Sin(angle) * maxAngle);
	}
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}
