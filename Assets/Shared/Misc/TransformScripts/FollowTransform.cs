using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
	#region variables
	public Transform target;
	#endregion
	#region initialization
	void Awake()
	{
		transform.parent = null;
	}
	void Start()
	{
		if (target != null)
			SetTarget(target);
	}
	#endregion
	#region logic
	private void Update()
	{
		if (target != null)
		{
			transform.position = target.position;
			transform.rotation = target.rotation;
		}
	}
	#endregion
	#region public interface
	public void SetTarget(Transform target)
	{
		this.target = target;
		transform.position = target.position;
		transform.rotation = target.rotation;
	}
	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}