using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
	#region variables
	public TileView tilePrefab;
	#endregion
	#region initialization
	public void StartSpawning()
	{
		StartCoroutine(SpawnC());
	}
	#endregion
	#region logic
	IEnumerator SpawnC()
	{
		yield return new WaitForSeconds(20f);
		
		while (true)
		{
			yield return new WaitForSeconds(Helpers.Rand(0.3f, 1.1f));

			int rx = Helpers.Rand(1, 15); //Lazy magic number

			var tile = Instantiate(tilePrefab, new Vector3(rx, 10, 0), Quaternion.identity);
			tile.posX = rx;
			tile.SetFalling();

			if (Helpers.RandPercent(30))
			{
				tile = Instantiate(tilePrefab, new Vector3(rx, 11, 0), Quaternion.identity);
				tile.posX = rx;
				tile.SetFalling();
			}

		}
	}
	#endregion
	#region public interface
	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}