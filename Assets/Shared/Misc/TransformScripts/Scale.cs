using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scale : MonoBehaviour
{
	#region vars

	public bool growOnStart = false;

	public Vector3 minScale = Vector3.zero;
	public Vector3 maxScale = Vector3.one;
	public float growDuration = 1f, shrinkDuration = 1f;
	public AnimationCurve growCurve;
	public AnimationCurve shrinkCurve;
	public bool unscaledTime = false;
	public bool destroyOnShrink = false;

	public bool scaling { get; private set; }

	public UnityEvent onDestroyStartEvent;

	private CoroutineManager scaleCM;

	#endregion

	#region init

	private void Awake()
	{
		scaleCM = new CoroutineManager(this);
	}

	private void Start()
	{
		if (growOnStart)
		{
			scaling = true;
			transform.localScale = minScale;
			Grow(0);
		}
	}

	#endregion

	#region logic

	#endregion

	#region public interface

	public void Stop()
	{
		scaleCM.Stop();
	}

	public void Grow(float delay = 0, System.Action onEndAction = null)
	{
		scaleCM.Start(scaleCoroutine(maxScale, growDuration, growCurve, delay, onEndAction));
	}

	public void Shrink(float delay = 0, System.Action onEndAction = null)
	{
		scaleCM.Start(scaleCoroutine(minScale, shrinkDuration, shrinkCurve, delay, onEndAction));
	}

	public void ChangeScale(Vector3 scale, float speed, AnimationCurve curve, float delay = 0, System.Action onEndAction = null)
	{
		scaleCM.Start(scaleCoroutine(scale, speed, curve, delay, onEndAction));
	}

	public void SetScale(Vector3 scale)
	{
		transform.localScale = scale;
	}

	public void ResetToMax()
	{
		transform.localScale = maxScale;
	}

	public void ResetToMin()
	{
		transform.localScale = minScale;
	}

	#endregion

	#region private interface

	private IEnumerator scaleCoroutine(Vector3 targetScale, float duration, AnimationCurve curve, float delay = 0, System.Action onEndAction = null)
	{
		if (destroyOnShrink && targetScale == minScale)
		{
			onDestroyStartEvent.Invoke();
		}

		if (delay > 0)
			yield return new WaitForSeconds(delay);

		scaling = true;
		float percent = 0;
		Vector3 currentScale = transform.localScale;

		if (duration > 0)
		{
			while (percent < 1f)
			{
				yield return null;
				if (unscaledTime)
					percent += Time.unscaledDeltaTime / duration;
				else
					percent += Time.deltaTime / duration;

				transform.localScale = Helpers.LerpOvershoot(currentScale, targetScale, curve.Evaluate(percent));
			}
		}
		
		transform.localScale = targetScale;

		scaling = false;
		if (onEndAction != null)
			onEndAction();

		if (destroyOnShrink && targetScale == minScale)
		{
			Destroy(gameObject);
		}
	}

	#endregion

	#region events

	#endregion
}