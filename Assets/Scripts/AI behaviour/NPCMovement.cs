using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class NPCMovement : MonoBehaviour
{

    [SerializeField] public Transform lookFrom;
    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
    public Vector3 GetCurrentDestination { get; private set; }


    NavMeshAgent _navMeshagent;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Range(0, 360)] public float viewAngle;
    public float viewRadius;


    private Maze _mazeInstance;
    private MazeCell _currentCell;
    public List<MazeCell> _visitedPoints;
    private Animator _anim;
    private Vector3 _playerLastSight;
    public Vector3 playerLastSight
    {
        get { return _playerLastSight; }
    }


    public void Start()
    {
        _navMeshagent = this.GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _mazeInstance = FindObjectOfType<Maze>();
        _visitedPoints = new List<MazeCell>();
        _navMeshagent.updateRotation = false;

        StartCoroutine("FindTargetsWithDelay", 0.07f);

    }


    void LateUpdate()
    {

        //Rotate towards movement direction
        if (GetComponent<NPCController>().currentState != NPCController.State.Attacking)
        {
            Quaternion updatedRotation = Quaternion.LookRotation(_navMeshagent.velocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, updatedRotation, 6.0f * Time.deltaTime);
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindTargets();
        }
    }

    void FindTargets()
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

                    if (target.GetComponent<PlayerController>() && !target.GetComponent<Health>().IsAlive())
                        return;

                    visibleTargets.Add(target);

                }
            }
        }
    }

    //Returns a random position from maze
    public Vector3 GetRandomPoint()
    {
        if (_mazeInstance == null)
            return Vector3.zero;

        MazeCell randomCell;
        do
        {
            randomCell = _mazeInstance.GetCell(_mazeInstance.RandomCoordinates);

        } while (_visitedPoints.Contains(randomCell));

        _visitedPoints.Add(randomCell);
        return randomCell.transform.position;
    }


    public void Patrolling()
    {

        if (!_navMeshagent.isOnNavMesh || _navMeshagent.remainingDistance > 0.1f)
            return;
        
        if (!_navMeshagent.pathPending && _navMeshagent.remainingDistance <= 0.1f)
        {

            Vector3 randomDestination = GetRandomPoint();
            MoveToPoint(randomDestination, 0.25f);
        }


    }

    public void Chasing()
    {
        MoveToPoint(visibleTargets[0].position, 1.25f);

    }

    //Set navmesh destination
    public void MoveToPoint(Vector3 destination, float speed)
    {
        this.GetCurrentDestination = destination;
        _navMeshagent.speed = speed;
        _navMeshagent.isStopped = false;
        _navMeshagent.enabled = true;
        _navMeshagent.SetDestination(destination);

    }

    //Set NPC on random cell
    public void SetLocation(MazeCell cell)
    {
        if (_currentCell != null)
        {
            _currentCell.OnPlayerExited();
        }
        _currentCell = cell;
        _visitedPoints.Add(_currentCell);


        Vector3 newPosition = cell.transform.localPosition;
        newPosition.y = 0.1f;
        transform.localPosition = newPosition;



        _currentCell.OnPlayerEntered();
    }

    //Stop movement and reset destination
    public void StopMovement()
    {
        _navMeshagent.velocity = Vector3.zero;
        this.GetCurrentDestination = Vector3.zero;
        _navMeshagent.isStopped = true;
        _navMeshagent.ResetPath();

    }


}
