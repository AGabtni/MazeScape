using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
    public float distance = 5.0f;
    private Transform lastTarget;
    private Animator animAI;


    private MazeCell currentCell;
   

    private NavMeshAgent _navmeshAgent ;
    private enum State
    {

        Idle,
        Patrolling,
        Following,
        Attacking
    }


    State currentState;

    private void Start()
    {
        movementController = GetComponent<EnemyMovement>();
        animAI = GetComponent<Animator>();
        currentState = State.Idle;
        _navmeshAgent = GetComponent<NavMeshAgent>();
    
    }




    private void Update()
    {


        if (movementController.visibleTargets.Count > 0)
        {
            Vector3 targetPosition = movementController.GetLastPlayerPosition;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            if (Vector3.Distance(transform.localPosition,movementController.visibleTargets[0].position)> 0.5f)
            {
                currentState = State.Following;
                Following();

            }
            else
            {
                currentState = State.Attacking;
                movementController.ResetDestination();
                Attacking();
            }


            /*
            Transform currentTarget = movementController.visibleTargets[Random.Range(0,movementController.visibleTargets.Count-1)];
            Vector3 targetPosition = currentTarget.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            if (Vector3.Distance(targetPosition, transform.position) > distance)
            {
                movementController.SetDestination(targetPosition);


            }
            */
            //if(!_navmeshAgent.pathPending && _navmeshAgent.remainingDistance < 0.5f && currentState != State.Attacking)
            //    movementController.GotoNextPoint();



        }
        else
        {
            currentState = State.Patrolling;
            Patrolling();
        }


        animAI.SetBool("isPatrolling", currentState == State.Patrolling ? true : false);
        animAI.SetBool("isFollowing", currentState == State.Following ? true : false);
        animAI.SetBool("isAttacking", currentState == State.Attacking ? true : false);



    }




    void Patrolling()
    {

        if (!_navmeshAgent.isOnNavMesh || _navmeshAgent.remainingDistance > 0.5f)
            return;

        if(!_navmeshAgent.pathPending && _navmeshAgent.remainingDistance < 0.5f)
        {
            _navmeshAgent.speed = 0.5f;
            movementController.GoToRandomPoint();

        }
            

    }

    void Attacking()
    {
        /*
        animAI.SetTrigger("jab");
        if(animAI.)
        animAI.ResetTrigger("jab");
        animAI.SetTrigger("cross");
        animAI.ResetTrigger("cross");

        animAI.SetTrigger("jab");
        animAI.ResetTrigger("jab");

        */

    }

    void Following ()
    {
        _navmeshAgent.speed = 1.0f;
        movementController.GotoNextPoint();

    }


   
}
