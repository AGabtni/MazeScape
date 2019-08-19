using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Maze mazePrefab;

	public PlayerMovement playerPrefab;
    public EnemyMovement aiPrefab;


	private Maze mazeInstance;

	private PlayerMovement playerInstance;
    private EnemyMovement aiInstance;

    private GameObject agentsHolder;


    private void Awake()
    {
        agentsHolder = GameObject.Find("AgentsHolder");


        //Hide Joysticks when using editor
        #if !UNITY_EDITOR
                VariableJoystick[] joysticks = FindObjectsOfType<VariableJoystick>();
                if (joysticks.Length > 0)
                {
                    for (int i = 0; i < joysticks.Length; i++)
                    {
                        joysticks[i].gameObject.active = false;
                    }
                }



        #endif

    }


    private void Start() {

        InstantMaze();
        InstantiateAgents();
            

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
        //Camera.main.clearFlags = CameraClearFlags.Skybox;
        //Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.InstantGenerate();
        playerInstance = Instantiate(playerPrefab) as PlayerMovement;
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        //Camera.main.clearFlags = CameraClearFlags.Depth;
        //Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);

        //Material mat = new Material()


      


    }

    private void InstantiateAgents()
    {

        int roomsCount = 0;

        for (int i = 0; i < mazeInstance.rooms.Count; i++)
        {


            if (mazeInstance.rooms[i].CellsNumber == 1)
            {
                MazeCell room = mazeInstance.rooms[i].cells[0];
                roomsCount++;
                room.transform.GetComponentInChildren<MeshRenderer>().material = Resources.Load("Materials/_Hologram_Rim_Flicker_Blue") as Material;

            }

        }


        for(int i=0; i< 50; i++)
        {


            EnemyMovement aiInstance = Instantiate(aiPrefab) as EnemyMovement;
            aiInstance.SetLocation(mazeInstance.rooms[i].RandomCell);
            aiInstance.GetComponentInChildren<SkinnedMeshRenderer>().material = Resources.Load("Materials/_Hologram_Rim_Flicker_Blue") as Material;

        }



    }

	private void RestartGame () {
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
		}
        InstantMaze();
	}
}