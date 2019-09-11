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

    

    Animator anim;
    NavMeshAgent _navMeshagent;
    new Rigidbody rigidbody;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

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
        StartCoroutine("FindTargetsWithDelay", 0.07f);
       
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
                    visibleTargets.Add(target);

                }
            }
        }
    }

    


    public Vector3 GoToRandomPoint()
    {
        if (mazeInstance == null)
            return Vector3.zero;

        MazeCell randomCell;
        do
        {
            randomCell = mazeInstance.GetCell(mazeInstance.RandomCoordinates);

        } while (visitedPoints.Contains(randomCell));

        visitedPoints.Add(randomCell);
        MoveToPoint( randomCell.transform.position);


        return randomCell.transform.position;
    }




    public void MoveToPoint(Vector3 destination)
    {
        this.GetCurrentDestination = destination;
        _navMeshagent.isStopped = false;
        _navMeshagent.enabled = true;
        _navMeshagent.SetDestination(destination);

    }

    public void ResetDestination()
    {
        this.GetCurrentDestination = Vector3.zero;

        _navMeshagent.ResetPath();

    }

    public Vector3 GetCurrentDestination { get; private set; }

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
