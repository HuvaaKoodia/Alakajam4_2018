using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
	public float fallSpeed = 10f;
	public float jitterTimeMin = 1.0f, jitterTimeMax = 1.2f;
	public Jitter jitter;
	public bool falling = false, stationary = true;
	public bool canBeCrushed = false;
	public UnityEngine.Events.UnityEvent onCrush;
	public AudioPlayer impact;
	public ParticleSystem dusty;
	public GameObject graphicsParent;

	[HideInInspector]
	public int posX = -1, posY = -1, roomIndex = -1;

	private CoroutineManager fallingCM;

	void Awake()
	{
		fallingCM = new CoroutineManager(this);
		Reset();
	}

	public void Reset()
	{
		gameObject.SetActive(true);

		posX = -1;
		posY = -1;
		roomIndex = -1;

		if (jitter)
			jitter.enabled = false;
		if (dusty)
			dusty.Stop();

		falling = false;
		stationary = true;
		fallingCM.Stop();
	}

	public void SetJitterFalling()
	{
		if (jitter)
			jitter.enabled = true;
		fallingCM.Start(FallCoroutine());
	}

	public void Crush()
	{
		LevelGenerator.I.ClearTileToPos(this);
		onCrush.Invoke();
		Destroy(gameObject);
	}

	IEnumerator FallCoroutine()
	{
		stationary = false;
		//gameObject.layer = LayerMask.NameToLayer("TileMoving");
		if (jitter && jitter.enabled)
		{
			yield return new WaitForSeconds(Helpers.Rand(jitterTimeMin, jitterTimeMax));
			jitter.ResetPos();
			jitter.enabled = false;
		}

		falling = true;

		if (roomIndex != -1)
		{
			LevelGenerator.I.ClearTileToPos(this);

			//drop tiles above this one
			int testRoomIndex = this.roomIndex;
			int testTileY = posY + 1;

			while (true)
			{
				if (testTileY == 0)
				{
					testRoomIndex += 1;
					testTileY = 4;
				}

				if (testTileY > 4)
				{
					testTileY = 1;
					testRoomIndex -= 1;

					if (testRoomIndex < 0)
						break;
				}

				var testRoom = LevelGenerator.I.rooms[testRoomIndex];

				var testTile = testRoom.tileTable[posX, testTileY];
				if (testTile != null && testTile.stationary)
					testTile.SetFalling();
				else
					break;

				testTileY += 1;
			}
		}

		while (true)
		{
			float distanceToPlayer = Mathf.Abs(transform.position.y - MainController.I.player.transform.position.y);

			if (distanceToPlayer > 100)
			{
				Destroy();
				yield break;
			}

			distanceToPlayer = Mathf.Abs(distanceToPlayer);

			var hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 0.8f, 0), Vector3.down, 1.6f, LayerMasks.tile);

			if (hit)
			{ //Tile hit?
				var other = hit.transform.GetComponent<TileView>();
				if (other && other.canBeCrushed)
				{
					other.Crush();
				}
				else if (other && !other.falling)
				{ //Stop moving
					falling = false;
					stationary = true;

					if (LevelGenerator.I.SetTileToPos(this, other.posY, other.roomIndex))
						yield break;

					transform.position = new Vector3(posX, (-this.roomIndex * 4)+ posY, 0);

					if (distanceToPlayer < 8)
					{
						if (impact)
							impact.Play2DSound();
						if (dusty)
							dusty.Play();
					}
					yield break;
				}
			}

			{ //Move
				transform.position += Vector3.down * fallSpeed * Time.deltaTime;

				//Player hit
				var hitP = Physics2D.CircleCast(transform.position + Vector3.one * 0.5f, 0.4f, Vector3.down, 0.25f, LayerMasks.player);

				if (hitP)
				{
					var player = hitP.collider.GetComponent<PlayerView>();
					player.CheckCrushingDeath();
				}
			}

			yield return true;
		}
	}

	public void Destroy()
	{
		if (fallingCM != null)
			fallingCM.Stop();
		gameObject.SetActive(false);
	}

	public void SetFalling()
	{
		if (!gameObject.activeSelf) return;
		StartCoroutine(FallCoroutine());
	}

	public void FlipGraphics()
	{
		graphicsParent.transform.rotation = Quaternion.Euler(0, 180, 0);
	}
}