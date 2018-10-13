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

	public void GenerateStartRooms()
	{
		GenerateNextRoom("Start", false);
		GenerateNextRoom("Base");
		GenerateNextRoom("Base");
	}

    public void GenerateNextRoom()
    {
        GenerateNextRoom("Base");
    }
    #endregion
    #region logic
    #endregion
    #region public interface
	
	private void GenerateNextRoom(string roomType, bool randomRoom = true)
	{
        LevelDatabase.RoomData room;
		
		if (randomRoom)
		{
			int openingIndex = 1 << Helpers.Rand(3);
			if (lastRoomData != null)
			{
				if (lastRoomData.index == 1 << 0)
					openingIndex = 1 << Helpers.RandParam(1, 2);
				else if (lastRoomData.index == 1 << 1)
					openingIndex = 1 << Helpers.RandParam(0, 2);
				else if (lastRoomData.index == 1 << 2)
					openingIndex = 1 << Helpers.RandParam(0, 1);
			}

			room = LevelDatabase.I.GetRandomRoom(roomType, openingIndex);
		}
		else 
			room = LevelDatabase.I.GetOnlyRoom(roomType);

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
	}

    #endregion
    #region private interface
    #endregion
    #region events
    #endregion
}