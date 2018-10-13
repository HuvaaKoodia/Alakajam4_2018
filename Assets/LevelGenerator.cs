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
	int lastRoomType = 0;
	LevelDatabase.RoomData lastRoomData;

	public void Generate()
	{
		{
			
			GenerateNextRoom();
		}

		{
			GenerateNextRoom();
		}

		{
			GenerateNextRoom();
		}
	}

	private void GenerateNextRoom()
	{
		
		int type = Helpers.Rand(3);
		if (lastRoomData != null)
		{
			if (lastRoomType == 0)
				type = Helpers.RandParam(1, 2); 
			else if (lastRoomType == 1)
				type = Helpers.RandParam(0, 2);
			else if (lastRoomType == 2)
				type = Helpers.RandParam(0, 1);
		}
		
		var room = LevelDatabase.I.GetRandomRoom("Base", 1 << type);
		
		for (int i = 0; i < room.width; i++)
		{
			bool topCheckOn = false;
			if (lastRoomData != null)
				topCheckOn = lastRoomData.data[i, 0] == TileID.Empty;

			for (int j = room.height - 1; j >= 0; j--)
			{
				if (topCheckOn)
				{
					if (room.data[i, j] == TileID.Wall)
						continue;
					else
						topCheckOn = false;
				}
				
				var pos = new Vector3(i, -currentYPos + j);

				if (room.data[i, j] == TileID.Wall)
				{
					var tile = Instantiate(tilePrefab, pos, Quaternion.identity);
				}

			}

		}

		currentYPos += room.height - 1;
		lastRoomData = room;
		lastRoomType = type;
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