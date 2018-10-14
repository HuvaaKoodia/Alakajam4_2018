using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
	public float fallSpeed = 10f;
	public float jitterTimeMin = 1.0f, jitterTimeMax = 1.2f;
	public Jitter jitter;
	public bool falling = false;

	public void SetJitterFalling()
	{
		falling = true;
		jitter.enabled = true;
		StartCoroutine(FallCoroutine());
	}

	IEnumerator FallCoroutine()
	{
		//gameObject.layer = LayerMask.NameToLayer("TileMoving");
		if (jitter.enabled)
		{
			yield return new WaitForSeconds(Helpers.Rand(jitterTimeMin, jitterTimeMax));
			jitter.ResetPos();
			jitter.enabled = false;
		}

		while (true)
		{
			//Player hit
			var hitP = Physics2D.CircleCast(transform.position + Vector3.one * 0.5f, 0.4f, Vector3.down, 0.25f, LayerMasks.player);

			if (hitP)
			{
				var player = hitP.collider.GetComponent<PlayerView>();
				player.CheckCrushingDeath();
			}

			//Tile hit
			var hit = Physics2D.Raycast(transform.position + Vector3.one * 0.5f, Vector3.down, 1f, LayerMasks.tiles);

			if (hit)
			{
				transform.position = hit.transform.position + Vector3.up;
				LevelGenerator.I.SetTileToPos(this);
			}
			else
				transform.position += Vector3.down * fallSpeed * Time.deltaTime;

			yield return true;
		}
	}

	public void SetFalling()
	{
		StartCoroutine(FallCoroutine());
	}
}