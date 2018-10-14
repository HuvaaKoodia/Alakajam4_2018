using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Jitter : MonoBehaviour
{
    #region variables
    public float distance = 1;
	public float interval = 0.1f;

    Vector3 startPos;
	float timer;
    #endregion
    #region initialization
    private void Start()
    {
        startPos = transform.localPosition;
    }
    #endregion
    #region update logic
    private void Update()
    {
		if(distance > 0 && timer < Time.time)
		{
			timer = Time.time + interval;
			transform.localPosition = startPos + (Vector3)Random.insideUnitCircle * distance;
		}
    }
    #endregion
    #region public interface
	public void ResetPos()
	{
		transform.localPosition = startPos;
	}
	
	public void IncreaseDistanceOverTime(float targetDistance, float duration)
	{
		StartCoroutine(IncreaseCoroutine(targetDistance, duration));
	}
    #endregion
    #region private interface

	IEnumerator IncreaseCoroutine(float targetDistance, float duration)
	{
		float timer = duration;
		float startDistance = distance;
		while(timer > 0)
		{
			timer -= Time.deltaTime;

			distance = Mathf.Lerp(targetDistance, startDistance, timer/duration);

			yield return null;
		}
		distance = targetDistance;
	}
    #endregion
    #region events
    #endregion
}
