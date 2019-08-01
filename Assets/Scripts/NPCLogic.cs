using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
    public float distance = 5.0f;
    private Transform lastTarget;
    private Animator animAI;

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
    }




    private void Update()
    {


        if (movementController.visibleTargets.Count > 0)
        {
            currentState = State.Following;
            Transform currentTarget = movementController.visibleTargets[Random.Range(0,movementController.visibleTargets.Count-1)];
            Vector3 targetPosition = currentTarget.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            if (Vector3.Distance(targetPosition, transform.position) > distance)
            {
                movementController.SetDestination(targetPosition);


            }
           
        }
        else
        {
            currentState = State.Patrolling;
        }


        animAI.SetBool("isPatrolling", currentState == State.Patrolling ? true : false);
        animAI.SetBool("isFollowing", currentState == State.Following ? true : false);


    }
}
