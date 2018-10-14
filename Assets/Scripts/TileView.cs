using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
	public float fallSpeed = 10f;
	public float jitterTimeMin = 1.0f, jitterTimeMax = 1.2f;
	public Jitter jitter;
	public bool falling = false, moving = false;
	public bool canBeCrushed = false;
	public UnityEngine.Events.UnityEvent onCrush;
	public AudioPlayer impact;
	public GameObject graphicsParent;

	public void SetJitterFalling()
	{
		falling = true;
		jitter.enabled = true;
		StartCoroutine(FallCoroutine());
	}

	public void Crush()
	{
		enabled = false;
		onCrush.Invoke();
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

		moving = true;

		while (true)
		{
			float distanceToPlayer = Mathf.Abs(transform.position.y - MainController.I.player.transform.position.y);
			
			if (distanceToPlayer > 100)
			{
				enabled = false;
				Destroy(graphicsParent);
				yield break;
			}
			
			//Player hit
			
			var hitP = Physics2D.CircleCast(transform.position + Vector3.one * 0.5f, 0.4f, Vector3.down, 0.25f, LayerMasks.player);

			if (hitP)
			{
				var player = hitP.collider.GetComponent<PlayerView>();
				player.CheckCrushingDeath();
			}

			//Tile hit
			var hit = Physics2D.Raycast(transform.position + Vector3.one * 0.5f, Vector3.down, 1f, LayerMasks.tile);

			if (hit)
			{
				transform.position = hit.transform.position + Vector3.up;
				LevelGenerator.I.SetTileToPos(this);
				if (moving)
				{
					moving = false;

					var other = hit.transform.GetComponent<TileView>();
					if (other && !other.moving)
					{
						if (distanceToPlayer < 8) impact.Play2DSound();
					}
				}
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