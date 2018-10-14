using UnityEngine;

/// <summary>
/// Contains layer bit masks and indices.
/// </summary>
public class LayerMasks
{
	public static readonly int player = 1 << LayerMask.NameToLayer("Player");
	public static readonly int tile = 1 << LayerMask.NameToLayer("Tile");
	
    public static readonly int groundCheck = tile;
}