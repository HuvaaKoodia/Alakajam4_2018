using UnityEngine;

/// <summary>
/// Contains layer bit masks and indices.
/// </summary>
public class LayerMasks
{
	public static readonly int entity = 1 << LayerMask.NameToLayer("Player");
	public static readonly int artView = 1 << LayerMask.NameToLayer("Block");
}