using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidboryExplosion : MonoBehaviour 
{
#region variables
public Rigidbody2D[] bodies;
public float explosiveForce;
public Transform explosionCenter;
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
		//var collider = body.GetComponent<Collider2D>();
		//collider.enabled = false;
		body.GetComponent<Scale>().Shrink();
		body.simulated = true;
		body.transform.SetParent(null);
		body.AddForceAtPosition(explosionCenter.To(body.transform) * explosiveForce, transform.position, ForceMode2D.Impulse);
	}
}
#endregion
#region private interface
#endregion
#region events
#endregion
}
