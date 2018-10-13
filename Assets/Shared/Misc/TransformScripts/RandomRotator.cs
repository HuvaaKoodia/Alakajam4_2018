using UnityEngine;
using System.Collections.Generic;

public class RandomRotator : MonoBehaviour 
{
    #region variables
    public Transform target;
    public float[] angles = { 0, 90, 180, 270 };
#endregion
#region initialization
	private void Start () 
	{
        target.rotation = Quaternion.AngleAxis(Helpers.RandParam(angles), Vector3.forward);
    }
#endregion
#region update logic
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}
