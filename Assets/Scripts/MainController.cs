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

		player.transform.position = new Vector3(2, -2);
		started = true;

	}
	#endregion
	#region logic

	float currentFloorLevel = -4f;
	bool started = false;

	void Update()
	{
		if (Input.GetButtonDown("Restart"))
		{
			Helpers.ReloadScene();
		}
		
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