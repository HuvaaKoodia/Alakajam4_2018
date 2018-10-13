using UnityEngine;

public class CircleAround : MonoBehaviour
{
	#region variables

	public float speed = 1, distanceX = 1, distanceY = 1, angleMultiX = 1f, angleMultiY = 1f;
	public bool randomStartAngle = false;
	float angle = 0;
	Vector3 offset;

	#endregion
	void Start() 
	{
		offset = transform.localPosition;
		if(randomStartAngle)
			angle = Helpers.Rand(360);
		
	}
	#region initialization

	#endregion

	#region update logic

	private void Update()
	{
		angle += Time.deltaTime * speed;
		transform.localPosition = offset + new Vector3(Mathf.Cos(angle * angleMultiX) * distanceX, Mathf.Sin(angle * angleMultiY) * distanceY, 0);
	}

	#endregion

	#region public interface

	#endregion

	#region private interface

	#endregion

	#region events

	#endregion
}
