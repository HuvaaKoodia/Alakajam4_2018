using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayScale : MonoBehaviour
{
	#region variables
	public float speed = 1f;
	public float minScale = 0.5f, maxScale = 1f;
	public Vector3 axis;

	float angle;
	#endregion
	#region initialization
	#endregion
	#region logic
	void Update()
	{
		angle += Time.deltaTime * speed;
		float scale = Mathf.Lerp(minScale, maxScale, ((1 + Mathf.Sin(angle)) / 2));

		float x = transform.localScale.x;
		float y = transform.localScale.y;
		float z = transform.localScale.z;

		if (axis.x != 0)
			x = scale * axis.x;
		if (axis.y != 0)
			y = scale * axis.x;
		if (axis.z != 0)
			z = scale * axis.x;

		transform.localScale = new Vector3(x, y, z);
}
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}