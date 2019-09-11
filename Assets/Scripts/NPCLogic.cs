using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
    public float attackDistance = 0.25f;
    private Vector3 playerLastSight;


    private Animator animAI;
    private List<AttackAnimationInfo> attackAnimations;
    private NavMeshAgent _navmeshAgent;
    private enum State
    {

        Idle,
        Patrolling,
        Chasing,
        Attacking
    }


    State currentState;


    public bool ikActive = false;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform lookObj = null;
    [SerializeField] private Transform lookAtObj = null;


    private void Start()
    {
        movementController = GetComponent<EnemyMovement>();
        animAI = GetComponent<Animator>();
        currentState = State.Idle;
        _navmeshAgent = GetComponent<NavMeshAgent>();



        attackAnimations = new List<AttackAnimationInfo>();
        attackAnimations.Add(new AttackAnimationInfo("jab", 0.375f));
        attackAnimations.Add(new AttackAnimationInfo("cross", 0.458f));

    }




    private void Update()
    {


        if (movementController.visibleTargets.Count > 0)
        {


            
            playerLastSight = movementController.visibleTargets[0].position;
            playerLastSight.y = transform.position.y;


            
            float distance = Vector3.Distance(playerLastSight, transform.position);
            transform.LookAt(playerLastSight);
            if (distance > 0.3f)
            {

                currentState = State.Chasing;
               
                Chasing();

            }
            else
            {
                currentState = State.Attacking;
                movementController.ResetDestination();
                Attacking();
            }


        }
        else
        {
            
            currentState = State.Patrolling;
            Patrolling();
        }


        animAI.SetBool("isPatrolling", currentState == State.Patrolling ? true : false);
        animAI.SetBool("isChasing", currentState == State.Chasing ? true : false);
        animAI.SetBool("isAttacking", currentState == State.Attacking ? true : false);



    }

    void Patrolling()
    {
        _navmeshAgent.speed = 0.25f;

        if (!_navmeshAgent.isOnNavMesh || _navmeshAgent.remainingDistance > 0.1f)
            return;

        if (!_navmeshAgent.pathPending && _navmeshAgent.remainingDistance <= 0.1f)
        {

            Vector3 destination = movementController.GoToRandomPoint();
            transform.LookAt(destination);
        }


    }

    public bool isAttacking = true;

    void Attacking()
    {
        if (isAttacking == false)
            return;

        StartCoroutine("randomAttack", attackAnimations[Random.Range(0, attackAnimations.Count)]);


    }


    IEnumerator randomAttack(AttackAnimationInfo attack)
    {
        isAttacking = false;

        animAI.SetTrigger(attack.title);
        yield return new WaitForSeconds(attack.duration);


        isAttacking = true;


    }

    void Chasing()
    {
        _navmeshAgent.speed = 1.0f;
        movementController.MoveToPoint(playerLastSight);

    }

    private void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {

            // Set the look target position, if one has been assigned

            if ((currentState == State.Attacking || currentState == State.Chasing)
                    && movementController.visibleTargets.Count > 0)
            {
                animAI.SetLookAtWeight(1);
                animAI.SetLookAtPosition(movementController.visibleTargets[0].position);

            }
            if (currentState == State.Patrolling && movementController.GetCurrentDestination != Vector3.zero)
            {

                animAI.SetLookAtWeight(1);
                animAI.SetLookAtPosition(lookAtObj.position);


            }
          

            /* Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                //_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animAI.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animAI.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);


                animAI.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                animAI.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

            }
            */

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && currentState == State.Attacking)
        {
            if (collision.contacts[0].thisCollider.name == "hand.R")
            {
                collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-2);
                Debug.Log("CROSS THROWN");
            }
            if (collision.contacts[0].thisCollider.name == "hand.L")
            {
                Debug.Log("JAB THROWN");

                collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-1);
            }


        }

    }




    

}


public class AttackAnimationInfo
{

    public string title;
    public float duration;


    public AttackAnimationInfo(string t, float d)
    {
        title = t;
        duration = d;

    }
}
