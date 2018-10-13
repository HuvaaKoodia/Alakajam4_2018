using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;

public enum TileID
{
    None = -1,
    Empty = 0,
    Wall,
}

public class LevelDatabase : MonoBehaviour
{
    #region variables
    public static LevelDatabase I;

    public string[] datafiles;
    public Dictionary<string, List<RoomData>[]> rooms;
    public bool loadingDone = false;

    private int[] roomTypes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
    private int[] roomTypeWeights = { 40, 5, 5, 3, 5, 3, 3, 1, 5, 3, 3, 1, 1, 1, 1, 1 };
    private int roomTypesTotalWeights = 0;
    int roomMaxHeight = 0;

    public class RoomData
    {
        public TileID[,] data;
        public int width { get { return data.GetLength(0); } }
        public int height { get { return data.GetLength(1); } }
    }
    #endregion
    #region initialization
    private void Awake()
    {
		if (I != null)
		{
			Destroy(gameObject);
			return;
		}

        I = this;
		DontDestroyOnLoad(gameObject);

        for (int i = 0; i < roomTypeWeights.Length; i++)
        {
            roomTypesTotalWeights += roomTypeWeights[i];
        }

        //load rooms from data files
        rooms = new Dictionary<string, List<RoomData>[]>();

        StartCoroutine(LoadWWW());
    }

    private void AddRoomType(string type)
    {
        var array = new List<RoomData>[16];
        rooms.Add(type, array);

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new List<RoomData>();
        }
    }

    public TileID[,] rotateArrayLeft(TileID[,] matrix)
    {
        /* W and H are already swapped */
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);
        var result = new TileID[h, w];
        for (int i = 0; i < h; ++i)
        {
            for (int j = 0; j < w; ++j)
            {
                result[i, j] = matrix[j, h - i - 1];
            }
        }
        return result;
    }

    #endregion
    #region update logic
    #endregion
    #region public interface
    public RoomData GetRandomRoom(string type, int index)
    {
        var list = rooms[type][index];
        return Helpers.Rand(list);
    }

    public int GetRandomRoomType()
    {
        return Helpers.RandWeighted(roomTypes, roomTypeWeights, roomTypesTotalWeights);
    }
    #endregion
    #region private interface

    private IEnumerator LoadWWW()
    {
        var container = new TextContainer();

        yield return GetTextContentWWW(datafiles, container);

        RoomData currentRoom = null;
        int currentVerticalIndex = 0, currentRoomIndex = 0;
        string currentRoomType = null;
        bool addRotations = false;

        foreach (var file in container.content)
        {
            var lines = Helpers.Split(file.Replace("\r", ""), "\n");

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];
                var lineSplit = Helpers.Split(line, ",");

                if (line.StartsWith("Room"))
                {
                    currentRoom = new RoomData();
                    currentVerticalIndex = 0;
                    var dataSplit = Helpers.Split(lineSplit[0], " ");

                    int roomIndex = 0;
                    addRotations = false;

                    if (dataSplit.Length > 4)
                    {
                        int startIndex = 2;
                        for (int i = startIndex; i < startIndex + 4; i++)
                        {
                            if (int.Parse(dataSplit[i]) == 1)
                            {
                                roomIndex = roomIndex | 1 << (i - startIndex);
                            }
                        }

                        if (dataSplit.Length > 6)
                            addRotations = dataSplit[6].ToLower() == "r";
                    }
                    else
                    {

                        roomIndex = int.Parse(dataSplit[2]);

                        if (dataSplit.Length > 3)
                            addRotations = dataSplit[3].ToLower() == "r";
                    }

                    string roomType = dataSplit[1];
                    if (!rooms.ContainsKey(roomType))
                    {
                        AddRoomType(roomType);
                    }

                    rooms[roomType][roomIndex].Add(currentRoom);
                    currentRoomType = roomType;
                    currentRoomIndex = roomIndex;

                    //calculate max height and width
                    roomMaxHeight = 0;
                    while (lineIndex + roomMaxHeight + 1 < lines.Length)
                    {
                        var nextLine = lines[lineIndex + roomMaxHeight + 1];
                        if (nextLine.StartsWith("Room") || nextLine.StartsWith(",") || string.IsNullOrEmpty(nextLine))
                            break;
                        roomMaxHeight++;
                    }

                    var splitt = Helpers.Split(lines[lineIndex + 1], ",");
                    int maxWidth = splitt.Length;

                    currentRoom.data = new TileID[maxWidth, roomMaxHeight];
                }
                else
                {
                    for (int i = 0; i < lineSplit.Length; i++)
                    {
                        var tile = lineSplit[i].ToLower();
                        TileID tileID = TileID.Empty;

                        if (tile == "w")
                            tileID = TileID.Wall;

                        currentRoom.data[i, roomMaxHeight - currentVerticalIndex - 1] = tileID;//hack vertical reversal
                    }
                    currentVerticalIndex++;

                    if (addRotations && currentVerticalIndex == roomMaxHeight) //hack magnum
                    {
                        //add rotations if needed
                        for (int i = 0; i < 3; i++)
                        {
                            var rotatedData = rotateArrayLeft(currentRoom.data);//rotate data 90 degrees
                            int newRoomType = (currentRoomIndex << 1 & 15) | (currentRoomIndex >> 3 & 15);//shift wrap type

                            var newRoom = new RoomData();
                            newRoom.data = rotatedData;

                            rooms[currentRoomType][newRoomType].Add(newRoom);
                            currentRoom = newRoom;
                            currentRoomIndex = newRoomType;
                        }
                    }
                }
            }
        }

        //temp = "length: " + rooms["Base"][0].Count;

        loadingDone = true;
    }

    private IEnumerator GetTextContentWWW(string[] paths, TextContainer container)
    {
        container.content = new string[paths.Length];

        for (int i = 0; i < paths.Length; i++)
        {
#if UNITY_STANDALONE //|| UNITY_EDITOR
			string filePath = "file://" +Path.Combine(Application.streamingAssetsPath, paths[i]);
            WWW www = new WWW(filePath);
            yield return www;
            container.content[i] = www.text;
#elif UNITY_ANDROID
            string filePath = "jar:file://" + Application.dataPath + "!/assets/"+ paths[i].Replace("\\", "/");
            WWW www = new WWW(filePath);
            yield return www;
            container.content[i] = www.text;
#elif UNITY_WEBGL
            string filePath = Path.Combine(Application.streamingAssetsPath, paths[i]).Replace("\\", "/");
            //temp = filePath;
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            //#if !UNITY_EDITOR
            //    var bytes = System.Text.Encoding.UTF8.GetBytes(www.downloadHandler.text);
            //    var text = System.Text.Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
            //    container.content[i] = text;
            //#else
            container.content[i] = www.downloadHandler.text;
            //#endif
            //temp += "\n"+ container.content[i];
#endif

        }
    }

    private class TextContainer
    {
        public string[] content;
    }

    #endregion
    #region events
    //string temp = "";
    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(10, 10, 1000, 50), temp);
    //}
    #endregion


}
