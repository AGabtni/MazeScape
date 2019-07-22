﻿using UnityEngine;
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


        if (Input.GetKeyDown(KeyCode.N))
        {

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

        int roomsCount = 0;
        //Material mat = new Material()
       for(int i=0; i< mazeInstance.rooms.Count; i++)
        {

            if(mazeInstance.rooms[i].CellsNumber == 1)
            {
                MazeCell room = mazeInstance.rooms[i].cells[0];
                //mazeInstance.rooms[i].cells[0].;
                roomsCount++;
                room.transform.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/_Hologram_Rim_Flicker_Blue") as Material;

            }
        }

        Debug.Log("There is " + roomsCount + " rooms with 1 cell");

        //Debug.Log("THERE IS " + mazeInstance.RoomsNumber + "ROOMS IN THIS MAZE");


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