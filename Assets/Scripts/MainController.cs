using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	#region variables
	public static MainController I;

	public PlayerView player;
	#endregion
	#region initialization
	private void Awake()
	{
		I = this;

	}

	IEnumerator Start()
	{
		while (!LevelDatabase.I.loadingDone)
			yield return null;

		LevelGenerator.I.GenerateStartRooms();

		player.transform.position = new Vector3(2, 3);
		started = true;

		StartCoroutine(TileFallC(0, 0.5f, 1.5f));
		StartCoroutine(TileFallC(1, 1.0f, 2.0f));
		StartCoroutine(TileFallC(2, 1.5f, 2.5f));
	}
	#endregion
	#region logic

	float currentFloorLevel = 0;
	bool started = false;

	void Update()
	{
		if (started && player.transform.position.y < currentFloorLevel)
		{
			CameraController.I.MoveDownOneFloor();
			LevelGenerator.I.GenerateNextRoom();
			currentFloorLevel -= 4f;
		}
	}
	#endregion
	#region public interface
	#endregion
	#region private interface

	public IEnumerator TileFallC(int roomIndex, float timeMin, float timeMax)
	{
		while (true)
		{
			yield return new WaitForSeconds(Helpers.Rand(timeMin, timeMax));

			var topFloor = LevelGenerator.I.rooms[roomIndex];
			int index = Helpers.Rand(1, topFloor.width - 2);

			for (int i = 0; i < 50; i++)
			{ //ugly hack
				if (topFloor.tileTable[index, topFloor.height - 1] == null)
				{
					index = Helpers.Rand(1, topFloor.width - 2);
				}
				else
					break;
			}

			for (int i = 3; i < topFloor.height; i++)
			{
				if (topFloor.tileTable[index, i] != null)
				{
					topFloor.tileTable[index, i].Fall();
					topFloor.tileTable[index, i] = null;
				}
			}
		}
	}

	#endregion
	#region events
	#endregion
}