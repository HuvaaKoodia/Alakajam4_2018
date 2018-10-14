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
		public int startX, startY;

		public int width { get { return tileTable.GetLength(0); } }
		public int height { get { return tileTable.GetLength(1); } }

		bool falling = false, empty = false;
		int fallingSpeedIndex = 1;
		float timer;

		public void SetFalling(int speedIndex)
		{
			fallingSpeedIndex = speedIndex;
			if (!falling)
			{
				falling = true;

				UpdateTimer();
			}

		}

		private void UpdateTimer()
		{
			if (fallingSpeedIndex == 3)
				timer = Helpers.Rand(0.5f, 1.5f);
			else if (fallingSpeedIndex == 2)
				timer = Helpers.Rand(1.0f, 2.0f);
			else if (fallingSpeedIndex == 1)
				timer = Helpers.Rand(1.5f, 2.5f);
		}

		public void Update()
		{
			if (empty)
				return;

			timer -= Time.deltaTime;

			if (timer < 0)
			{
				UpdateTimer();

				var availableRooms = new List<int>();
				for (int i = 1; i < width - 1; i++)
				{
					if (tileTable[i, height - 1] != null)
						availableRooms.Add(i);
				}

				if (availableRooms.Count == 0)
				{
					empty = false;
					return;
				}

				int x = Helpers.Rand(availableRooms);

				if (tileTable[x, 4] != null)
				{
					tileTable[x, 4].SetJitterFalling();
					tileTable[x, 4] = null;

					//if anything above it, drop it too
					if (index - 1 >= 0)
					{
						var room2 = LevelGenerator.I.rooms[index - 1];
						for (int i = 0; i < room2.height - 1; i++)
						{
							if (room2.tileTable[x, i] != null && !room2.tileTable[x, i].falling)
								room2.tileTable[x, i].SetFalling();

						}
					}
				}
			}
		}
	}

	#region variables
	public static LevelGenerator I;

	public List<RoomView> rooms = new List<RoomView>();
	public TileView wallPrefab, floorPrefab;
	public GameObject BG;
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
		GenerateNextRoom("Start1", false);
		GenerateNextRoom("Start2", false);
		GenerateNextRoom("Start3", false);
		GenerateNextRoom("Base");
	}

	public void GenerateNextRoom()
	{
		GenerateNextRoom("Base");
	}
	#endregion
	#region logic
	void Update()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			rooms[i].Update();
		}

	}
	#endregion
	#region public interface

	int roomIndex = 0;

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

		var roomView = new RoomView();
		roomView.startY = -currentYPos;
		roomView.index = roomIndex++; //Debug only
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
					var prefab = floorPrefab;
					if (i == 0 ||  i == room.width - 1)
						prefab = wallPrefab;

					var tile = Instantiate(prefab, pos, Quaternion.identity);
					roomView.tileTable[i, j] = tile;
				}
			}
		}

		//Steal top row from last room view.... Ugly as heck!
		if (rooms.Count > 0)
		{
			var upperRoom = rooms[rooms.Count - 1];
			for (int i = 1; i < upperRoom.width - 1; i++)
			{
				roomView.tileTable[i, roomView.height - 1] = upperRoom.tileTable[i, 0];
			}
		}

		//Set falling speeds
		if (rooms.Count >= 3)
		{
			for (int i = rooms.Count - 3; i < rooms.Count; i++)
			{
				Debug.Log(3 - (i - (rooms.Count - 3)));
				rooms[i].SetFalling(3 - (i - (rooms.Count - 3)));
			}
		}

		currentYPos += room.height - 1;
		lastRoomData = room;
		rooms.Add(roomView);

		roomCount++;

		if (roomCount % 3 == 0)
			Instantiate(BG, BG.transform.position + (Vector3.down * 13 * Mathf.Floor(roomCount / 3f)), Quaternion.identity);
	}

	public void SetTileToPos(TileView tileView)
	{
		int x = (int)tileView.transform.position.x;
		int y = (int)tileView.transform.position.y;

		for (int i = 0; i < rooms.Count; i++)
		{
			if (y >= rooms[i].startY)
			{
				var yOff = y - rooms[i].startY;
				if (yOff >= rooms[i].startY + rooms[i].height)return;

				rooms[i].tileTable[x, yOff] = tileView;
			}
		}
	}

	int roomCount = 0;

	#endregion
	#region private interface
	#endregion
	#region events
	#endregion
}