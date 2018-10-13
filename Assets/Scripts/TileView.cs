using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileView : MonoBehaviour
{
	public float fallSpeed = 10f;
	public Jitter jitter;

	public void Fall()
	{
		StartCoroutine(FallCoroutine());
	}

	IEnumerator FallCoroutine()
	{
		jitter.enabled = true;
		//gameObject.layer = LayerMask.NameToLayer("TileMoving");

		yield return new WaitForSeconds(1.5f);

		while (true)
		{
			
			var hit = Physics2D.Raycast(transform.position + Vector3.one * 0.5f, Vector3.down, 1f, LayerMasks.tiles);

			if (hit)
			{
				transform.position = hit.transform.position + Vector3.up;
				jitter.enabled = false;
				jitter.ResetPos();
			}
			else
				transform.position += Vector3.down * fallSpeed * Time.deltaTime;

			yield return true;
		}
	}
}