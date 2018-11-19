using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
This code is not pretty, it is hanging by a thread.
The tile incides and tables are all messed up.  
*/
public class LevelGenerator : MonoBehaviour
{
	public class RoomView
	{ //Hacks
		public int index = 0;
		public TileView[, ] tileTable;
		public int startX, startY;

		public int width { get { return tileTable.GetLength(0); } }
		public int height { get { return tileTable.GetLength(1); } }

		public Vector2 posOffset;

		bool falling = false;
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
			timer -= Time.deltaTime;

			if (timer < 0)
			{
				UpdateTimer();

				var availableRooms = new List<int>();
				for (int i = 1; i < width - 1; i++)
				{
					if (tileTable[i, height - 1] != null && tileTable[i, height - 2] == null && tileTable[i, height - 1].stationary)
						availableRooms.Add(i);
				}

				if (availableRooms.Count == 0)
				{
					timer = 10f;
					return;
				}

				int x = Helpers.Rand(availableRooms);

				if (tileTable[x, 4] != null && tileTable[x, 3] == null)
				{
					tileTable[x, 4].SetJitterFalling();
					//tileTable[x, 4] = null;
				}
			}
		}
	}

	#region variables
	public static LevelGenerator I;

	public List<RoomView> rooms = new List<RoomView>();
	public TileView wallPrefab, floorPrefab, statuePrefab;
	public GameObject BG;

	public Sprite[] smallDecals, bigDecals, hugeDecals;
	public SpriteRenderer decalPrefab;
	public int minDecalPerRoom = 1, maxDecalPerRoom = 5;

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
	Vector2Int[] bigDecalPositions = new Vector2Int[]
	{
		new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1)
	};

	Vector2Int[] hugeDecalPositions = new Vector2Int[]
	{
		new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1),
			new Vector2Int(2, 0), new Vector2Int(1, 2), new Vector2Int(0, 2),
			new Vector2Int(1, 2), new Vector2Int(2, 2)
	};

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

		bool[, ] decalPositionsFreeTable = new bool[room.width, room.height];
		List<Vector2Int> decalPositionsFreeList = new List<Vector2Int>();

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

				if (room.data[i, j] > TileID.Empty)
				{
					var prefab = floorPrefab;
					TileView tile = null;

					bool flipGraphics = false;
					if (room.data[i, j] == TileID.StatueL)
						prefab = statuePrefab;
					if (room.data[i, j] == TileID.StatueR)
					{
						flipGraphics = true;
						prefab = statuePrefab;
					}
					else if (i == 0 ||  i == room.width - 1)
						prefab = wallPrefab;

					tile = Instantiate(prefab, pos, Quaternion.identity);
					roomView.tileTable[i, j] = tile;
					tile.posX = i;
					tile.posY = j;
					tile.roomIndex = roomView.index;

					if (flipGraphics)
						tile.FlipGraphics();
				}
				else if (i > 0 && i < room.width - 1 && j > 0 && j < room.height - 1)
				{
					decalPositionsFreeTable[i, j] = true;
					decalPositionsFreeList.Add(new Vector2Int(i, j));
				}
			}
		}

		//Add decals
		int decalAmount = Helpers.Rand(minDecalPerRoom, maxDecalPerRoom);

		for (int i = 0; i < decalAmount; i++)
		{
			if (decalPositionsFreeList.Count == 0)break;

			var position = Helpers.RandRemove(decalPositionsFreeList);
			decalPositionsFreeTable[position.x, position.y] = false;

			bool canAddBigDecal = true, canAddHugeDecal = true;

			foreach (var pos in bigDecalPositions)
			{
				int x = position.x + pos.x, y = position.y + pos.y;

				if (room.data[x, y] != TileID.Empty || !decalPositionsFreeTable[x, y])
				{
					canAddBigDecal = false;
					canAddHugeDecal = false;
					break;
				}
			}

			if (canAddBigDecal)
			{
				foreach (var pos in hugeDecalPositions)
				{
					int x = position.x + pos.x, y = position.y + pos.y;

					if (room.data[x, y] != TileID.Empty || !decalPositionsFreeTable[x, y])
					{
						canAddHugeDecal = false;
						break;
					}
				}
			}

			if (canAddHugeDecal && Helpers.RandPercent(40))
			{
				var decal = Instantiate(decalPrefab, (Vector2)position + Vector2.up * roomView.startY, Quaternion.identity)as SpriteRenderer;
				decal.sprite = Helpers.Rand(hugeDecals);

				foreach (var pos in hugeDecalPositions)
				{
					int x = position.x + pos.x, y = position.y + pos.y;
					decalPositionsFreeTable[x, y] = false;

					for (int j = 0; j < decalPositionsFreeList.Count; j++)
					{ //Ugly hack
						if (decalPositionsFreeList[j].x == x && decalPositionsFreeList[j].y == y)
							decalPositionsFreeList.RemoveAt(j);
					}
				}
			}
			else
			if (canAddBigDecal)
			{
				var decal = Instantiate(decalPrefab, (Vector2)position + Vector2.up * roomView.startY, Quaternion.identity)as SpriteRenderer;
				decal.sprite = Helpers.Rand(bigDecals);

				foreach (var pos in bigDecalPositions)
				{
					int x = position.x + pos.x, y = position.y + pos.y;
					decalPositionsFreeTable[x, y] = false;

					for (int j = 0; j < decalPositionsFreeList.Count; j++)
					{ //Ugly hack
						if (decalPositionsFreeList[j].x == x && decalPositionsFreeList[j].y == y)
							decalPositionsFreeList.RemoveAt(j);
					}
				}
			}
			else
			{
				if (smallDecals.Length == 0)
				{ //Try again in different position
					i--;
					continue;
				}

				var decal = Instantiate(decalPrefab, (Vector2)position + Vector2.up * roomView.startY, Quaternion.identity)as SpriteRenderer;
				decal.sprite = Helpers.Rand(smallDecals);
			}
		}

		//Steal top row from last room view.... Ugly as heck!
		if (rooms.Count > 0)
		{
			var upperRoom = rooms[rooms.Count - 1];
			for (int i = 1; i < upperRoom.width - 1; i++)
			{
				var upperTile = upperRoom.tileTable[i, 0];

				if (upperTile != null)
				{
					upperRoom.tileTable[i, 0] = null;
					roomView.tileTable[i, roomView.height - 1] = upperTile;

					upperTile.posY = roomView.height - 1;
					upperTile.roomIndex = roomView.index;
				}
			}
		}

		//Set falling speeds
		if (rooms.Count >= 3)
		{
			for (int i = rooms.Count - 3; i < rooms.Count; i++)
			{
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

	public bool SetTileToPos(TileView tileView, int otherY, int otherRoomIndex)
	{
		int x = tileView.posX;
		int y = otherY + 1;

		{
			if (y > 4)
			{
				otherRoomIndex -= 1;

				y = y - 4;

				if (otherRoomIndex < 0)
				{
					tileView.Destroy();
					return true;
				}
			}

			rooms[otherRoomIndex].tileTable[x, y] = tileView;
			tileView.posX = x;
			tileView.posY = y;
			tileView.roomIndex = otherRoomIndex;
		}

		return false;
	}

	public void ClearTileToPos(TileView tileView)
	{
		int x = tileView.posX;
		int y = tileView.posY;

		if (y == 0)
		{
			y = 4;
			tileView.roomIndex += 1;
		}

		if (y > 4)
		{
			tileView.roomIndex -= 1;

			y = y - 4;

			if (tileView.roomIndex < 0)
			{
				tileView.Destroy();
				return;
			}
		}

		rooms[tileView.roomIndex].tileTable[tileView.posX, y] = null;
	}

	int roomCount = 0;

	#endregion
	#region private interface
	#endregion
	#region events
#if UNITY_EDITOR
	// void OnDrawGizmos()
	// {
	// 	if (rooms == null ||  rooms.Count == 0)
	// 		return;

	// 	for (int r = 0; r < 3; r++)
	// 	{
	// 		var room1 = rooms[r];

	// 		for (int x = 0; x < room1.width; x++)
	// 		{
	// 			for (int y = 1; y < room1.height; y++)
	// 			{
	// 				Gizmos.color = Color.white;
	// 				if (room1.tileTable[x, y] == null)
	// 					Gizmos.color = Color.red;

	// 				Gizmos.DrawCube(new Vector3(x + 0.5f, -r * 4 + y + 0.5f, 2), Vector3.one);

	// 				UnityEditor.Handles.color = Color.black;
	// 				Gizmos.color = Color.black;
	// 				UnityEditor.Handles.Label(new Vector3(x + 0.1f, y + 0.5f, 0), x + ":" + y);
	// 			}
	// 		}
	// 	}
	// }
#endif
	#endregion
}