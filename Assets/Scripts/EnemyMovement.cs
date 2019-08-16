using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyMovement : MonoBehaviour
{

    [SerializeField]

    public Transform lookFrom;
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private Vector3 playerLastSight;
    Animator anim;
    NavMeshAgent _navMeshagent;
    Rigidbody rigidbody;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    private int destPoint = 0;

    int[] validChoices = new int[] { -1, 1 };

    private Maze mazeInstance;

    private MazeCell currentCell;
    public List<MazeCell> visitedPoints;



    public void Start()
    {
        _navMeshagent = this.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        mazeInstance = FindObjectOfType<Maze>();
        visitedPoints = new List<MazeCell>();
        StartCoroutine("FindTargetsWithDelay", 0.1f);

       
    }

    private void Update()
    {
     

        
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider[] targetsInViewRadius = Physics.OverlapSphere(lookFrom.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - lookFrom.position).normalized;
            if (Vector3.Angle(lookFrom.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(lookFrom.position, target.position);

                if (!Physics.Raycast(lookFrom.position, dirToTarget, dstToTarget, obstacleMask))
                {

                    //transform.position = Vector3.Slerp(transform.position, target.position, Time.time);
                    playerLastSight = target.position;
                    visibleTargets.Add(target);

                }
            }
        }
    }

    



    public void GotoNextPoint()
    {   
        _navMeshagent.destination = playerLastSight;
    }

    public void GoToRandomPoint()
    {
        if (mazeInstance == null)
            return;

        MazeCell randomCell;
        do
        {
            randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);

        } while (visitedPoints.Contains(randomCell));

        visitedPoints.Add(randomCell);
        _navMeshagent.destination = randomCell.transform.position;
    }

    public void ResetDestination()
    {
        _navMeshagent.ResetPath();


    }


    public Vector3 GetLastPlayerPosition
    {
        get
        {
            return playerLastSight;
        }
    }


    //SET LOCATION ON A CELL
    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        visitedPoints.Add(currentCell);


        Vector3 newPosition = cell.transform.localPosition;
        newPosition.y = 0.1f;
        transform.localPosition = newPosition;



        currentCell.OnPlayerEntered();
    }
}
