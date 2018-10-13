using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public class RoomView
	{ //Hacks
		public int index = 0;
		public TileView[, ] tileTable;
		public List<TileView> tiles = new List<TileView>();

		public int width { get { return tileTable.GetLength(0); } }
		public int height { get { return tileTable.GetLength(1); } }
	}

	#region variables
	public static LevelGenerator I;

	public List<RoomView> rooms = new List<RoomView>();
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

	int roomIndex = 0;

	private void GenerateNextRoom(string roomType, bool randomRoom = true)
	{
		if (rooms.Count == 3)
			rooms.RemoveAt(0);

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

		var roomView = new RoomView();
		roomView.index = roomIndex++;
		roomView.tileTable = new TileView[room.width, room.height];

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
					roomView.tiles.Add(tile);
					roomView.tileTable[i, j] = tile;
				}
			}
		}
		
		//Steal top row from last room view.... Ugly as heck!
		if (rooms.Count > 0)
		{
			var upperRoom = rooms[rooms.Count - 1];
			for (int i = 1; i < upperRoom.width - 2; i++)
			{
				roomView.tileTable[i, roomView.height - 1] = upperRoom.tileTable[i, 0];
			}
		}

		currentYPos += room.height - 1;
		lastRoomData = room;
		rooms.Add(roomView);
	}

	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}