using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{

    [SerializeField] private Maze mazePrefab;
    [SerializeField] private PlayerController playerPrefab;
    [SerializeField] private NPCMovement agentPrefab;
    [SerializeField] private int agentsNumber = 1;
    [SerializeField] private Transform[] weaponsPrefabs;
    private Maze _mazeInstance;

    private PlayerController _playerInstance;

    private GameObject _agentsHolder;

    private void Awake()
    {
        _agentsHolder = GameObject.Find("agentsHolder");


#if UNITY_ANDROID
        //Hide Joysticks when using editor
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


    private void Start()
    {
        InstantMaze();
    }

    //Instantiate Maze
    private void InstantMaze()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        _mazeInstance = Instantiate(mazePrefab) as Maze;
        _mazeInstance.InstantGenerate();
        _playerInstance = Instantiate(playerPrefab) as PlayerController;
        UserInput.instance.playerController = _playerInstance;
        _playerInstance.SetLocation(_mazeInstance.GetCell(_mazeInstance.RandomCoordinates));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(-0.2f, 0.05f, 0.5f, 0.5f);

        //Hide enemies on map
        int camMask = 1 << LayerMask.NameToLayer("Enemy");
        Camera.main.cullingMask = ~camMask;


        InstantiateAgents(50);
        InstantiateWeapons();
    }


    //Instantiate an agent on a random cell
    private void InstantiateAgents(int num)
    {
        for (int i = 0; i < num; i++)
        {
            NPCMovement aiInstance = Instantiate(agentPrefab) as NPCMovement;
            aiInstance.SetLocation(_mazeInstance.rooms[i].RandomCell);
            aiInstance.name = "Agent N°" + i;
        }
    }


    //Instantiate one random weapon per room
    private void InstantiateWeapons()
    {


        for (int i = 0; i < _mazeInstance.RoomsNumber; i++)
        {
            MazeCell cell = _mazeInstance.rooms[i].RandomCell;
            //Instantiate a random weapon with bullets
            if (weaponsPrefabs.Length > 0)
            {
                Transform randomWeapon = Instantiate(weaponsPrefabs[Random.Range(0, weaponsPrefabs.Length)], cell.transform);
                randomWeapon.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0.1f, Random.Range(-0.4f, 0.4f));

            }

        }

    }


    public void OnReplay()
    {
        UIManager.instance.ToggleGameMenu();

        StopAllCoroutines();
        Destroy(_mazeInstance.gameObject);
        if (_playerInstance != null)
        {
            Destroy(_playerInstance.gameObject);
        }
        InstantMaze();
    }

}