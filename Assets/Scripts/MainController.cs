using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour
{
#region variables
	public static MainController I;
	
	public PlayerView player;
	public LevelGenerator generator;
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
			
		generator.GenerateStartRooms();
		
		player.transform.position = new Vector3(2,3);
		started = true;
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
#endregion
#region events
#endregion
}
