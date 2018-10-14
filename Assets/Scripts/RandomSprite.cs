using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour 
{
#region variables
public Sprite[] sprites;
public SpriteRenderer r;
#endregion
#region initialization
	void Start () 
    {
		r.sprite = Helpers.Rand(sprites);
	}
#endregion
#region logic
#endregion
#region public interface
#endregion
#region private interface
#endregion
#region events
#endregion
}
