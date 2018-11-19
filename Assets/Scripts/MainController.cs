using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
	#region variables
	public static MainController I;

	public PlayerView player;
	public TileSpawner spawner;
	int score = 0;
	public Text scoreText;

	#endregion
	#region initialization
	private void Awake()
	{
		I = this;

		scoreText.text = "";
	}

	IEnumerator Start()
	{
		while (!LevelDatabase.I.loadingDone)
			yield return null;

		LevelGenerator.I.GenerateStartRooms();

		player.transform.position = new Vector3(2, -2);
		started = true;
		spawner.StartSpawning();

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
		
		if (Input.GetButtonDown("BackToMenu"))
		{
			Helpers.LoadScene("Menu");
		}

		if (started && player.transform.position.y < currentFloorLevel)
		{
			CameraController.I.MoveDownOneFloor();
			LevelGenerator.I.GenerateNextRoom();
			currentFloorLevel -= 4f;
			score++;

			scoreText.text = "" + score;

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