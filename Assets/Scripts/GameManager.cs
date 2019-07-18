using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	public PlayerMovement playerPrefab;

	private Maze mazeInstance;

	private PlayerMovement playerInstance;

	private void Start () {

        InstantMaze();
    }
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			RestartGame();
		}
	}

    private void InstantMaze()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.InstantGenerate();
        playerInstance = Instantiate(playerPrefab) as PlayerMovement;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);

       

    }

	private IEnumerator BeginGame () {
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
		mazeInstance = Instantiate(mazePrefab) as Maze;
		yield return StartCoroutine(mazeInstance.Generate());
		//playerInstance = Instantiate(playerPrefab) as Player;
		//playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
	}


	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
		}
        //StartCoroutine(BeginGame());
        InstantMaze();
	}
}