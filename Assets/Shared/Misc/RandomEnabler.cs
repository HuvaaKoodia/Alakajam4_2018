using UnityEngine;

public class RandomEnabler : MonoBehaviour 
{
    #region variables
    public Renderer[] renderers;
#endregion
#region initialization
	private void Start () 
	{
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = Helpers.RandBool();
        }
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
