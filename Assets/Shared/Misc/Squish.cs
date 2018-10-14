using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
	#region variables
	public AnimationCurve curve;
	CoroutineManager scaleCM;
	public float duration;
	#endregion
	#region initialization
	void Awake()
	{
		scaleCM = new CoroutineManager(this);
	}
	#endregion
	#region logic
	#endregion
	#region public interface
	public void SquishIt()
	{
		scaleCM.Start(scaleCoroutine());
	}
	#endregion
	#region private interface
	#endregion
	#region events
	private IEnumerator scaleCoroutine()
	{
		float percent = 0;
		var currentScaleY = transform.localScale.y;
		if (duration > 0)
		{
			while (percent < 1f)
			{
				yield return null;

				percent += Time.deltaTime / duration;
				
				if (percent > 1f) percent = 1f;

				transform.localScale = new Vector3(transform.localScale.x, currentScaleY * curve.Evaluate(percent), transform.localScale.z);
			}
		}

	}
	#endregion
}