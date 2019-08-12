using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
    public float attackDistance = 0.25f;
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

            if (Vector3.Distance(transform.localPosition,movementController.visibleTargets[0].position)> attackDistance)
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
        animAI.SetBool("isChasing", currentState == State.Following ? true : false);
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

    public bool isAttacking = true;
    void Attacking()
    {
        if (isAttacking == false)
            return;

        AnimatorStateInfo animationState = <.GetCurrentAnimatorStateInfo(0);
        AnimatorClipInfo[] myAnimatorClip = myAnimator.GetCurrentAnimatorClipInfo(0);
        float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;

        animAI.SetTrigger("cross"); animAI.SetTrigger("jab"); // animAI.SetTrigger("jab");
        //animAI.ResetTrigger("jab");
        isAttacking = false;
        /*
       
        if(animAI.)
       
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
