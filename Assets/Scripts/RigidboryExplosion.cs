using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidboryExplosion : MonoBehaviour 
{
#region variables
public Rigidbody2D[] bodies;
public float explosiveForce;
#endregion
#region initialization
#endregion
#region logic
#endregion
#region public interface
public void Explode()
{
	foreach (var body in bodies)
	{
		body.AddForceAtPosition(transform.To(body.transform), transform.position); 
	}
}
#endregion
#region private interface
#endregion
#region events
#endregion
}
