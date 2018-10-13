using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	#region variables
	public static LevelGenerator I;

	public TileView tilePrefab;
	#endregion
	#region initialization
	private void Awake()
	{
		I = this;
	}

	int currentYPos = 0;
	LevelDatabase.RoomData lastRoomData;

	public void Generate()
	{
		{
			var room = LevelDatabase.I.GetRandomRoom("Base", 0);
			GenerateNextRoom(room);
		}

		{
			var room = LevelDatabase.I.GetRandomRoom("Base", 0);
			GenerateNextRoom(room);
		}

		{
			var room = LevelDatabase.I.GetRandomRoom("Base", 0);
			GenerateNextRoom(room);
		}
	}

	private void GenerateNextRoom(LevelDatabase.RoomData room)
	{
		for (int i = 0; i < room.width; i++)
		{
			for (int j = 0; j < room.height; j++)
			{
				var pos = new Vector3(i, -currentYPos + j);

				if (j == 0 && lastRoomData.data[i, lastRoomData.data.GetLength(1) - 1] == TileID.None)
				{
					
				}

				if (room.data[i, j] == TileID.Wall)
				{
					var tile = Instantiate(tilePrefab, pos, Quaternion.identity);
				}
			}
		}

		currentYPos += room.height - 1;
		lastRoomData = room;
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