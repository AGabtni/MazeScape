using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

[RequireComponent(typeof(NPCMovement))]
public class NPCController : MonoBehaviour
{
    public float attackDistance = 1.0f;
    public bool ikActive = false;
    [SerializeField] private Transform lookAtObj = null;

    public enum State
    {

        Idle,
        Patrolling,
        Chasing,
        Attacking
    }


    private State _currentState;
    public State currentState
    {
        get { return _currentState; }
    }

    private NPCMovement _movementController;
    private MeshDestroy _meshDestroyer;
    private Animator _animAI;
    private Health _health;
    private NavMeshAgent _navmeshAgent;
    private List<AttackAnimationInfo> attackAnimations;
    private bool _isAttacking = true;
    private bool _isAlive = true;





    private void Start()
    {
        _currentState = State.Idle;

        _movementController = GetComponent<NPCMovement>();
        _meshDestroyer = GetComponentInChildren<MeshDestroy>();
        _animAI = GetComponent<Animator>();
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();


        attackAnimations = new List<AttackAnimationInfo>();
        attackAnimations.Add(new AttackAnimationInfo("jab", 0.375f));
        attackAnimations.Add(new AttackAnimationInfo("cross", 0.458f));

    }




    private void Update()
    {
        if (!_health.IsAlive())
        {
            OnDeath();
            return;
        }

        if (_movementController.visibleTargets.Count > 0)
        {
            Vector3 playerPosition = _movementController.visibleTargets[0].position;
            playerPosition.y = transform.position.y;

            if (Vector3.Distance(playerPosition, transform.position) > attackDistance)
            {
                _currentState = State.Chasing;
                _movementController.Chasing();
            }
            else
            {
                _currentState = State.Attacking;
                Attacking();
            }
            transform.LookAt(playerPosition);

        }
        else
        {
            _currentState = State.Patrolling;
            _movementController.Patrolling();
        }
    }

    void LateUpdate()
    {
        _animAI.SetBool("isPatrolling", _currentState == State.Patrolling ? true : false);
        _animAI.SetBool("isChasing", _currentState == State.Chasing ? true : false);
        _animAI.SetBool("isAttacking", _currentState == State.Attacking ? true : false);
    }


    void Attacking()
    {
        if (_isAttacking == false)
            return;
        _isAttacking = false;
        _movementController.StopMovement();
        StartCoroutine("RandomAttack", attackAnimations[Random.Range(0, attackAnimations.Count)]);


    }


    IEnumerator RandomAttack(AttackAnimationInfo attack)
    {

        _animAI.SetTrigger(attack.title);
        yield return new WaitForSeconds(attack.duration);
        _isAttacking = true;


    }



    private void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {

            // Set the look target position, if one has been assigned

            if ((_currentState == State.Attacking || _currentState == State.Chasing)
                    && _movementController.visibleTargets.Count > 0)
            {
                _animAI.SetLookAtWeight(1);
                _animAI.SetLookAtPosition(_movementController.visibleTargets[0].position);

            }
            if (_currentState == State.Patrolling && _movementController.GetCurrentDestination != Vector3.zero)
            {
                if (lookAtObj != null)
                {
                    _animAI.SetLookAtWeight(1);
                    _animAI.SetLookAtPosition(lookAtObj.position);
                }

            }


            /* Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                //_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animAI.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animAI.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);


                _animAI.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                _animAI.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

            }
            */

        }
    }
    void OnDeath()
    {
        if (!_isAlive)
            return;
        _currentState = State.Idle;
        _isAlive = false;
        _meshDestroyer.DestroyMesh();
        _movementController.StopMovement();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && _currentState == State.Attacking)
        {
            if (collision.contacts[0].thisCollider.name == "hand.R")
            {
                //Debug.Log("CROSS THROWN");

                collision.gameObject.GetComponent<Health>().ChangeHealth(-15);
                UIManager.instance.UpdateHealth.Invoke(collision.gameObject.GetComponent<Health>());

            }
            else if (collision.contacts[0].thisCollider.name == "hand.L")
            {
                //Debug.Log("JAB THROWN");

                collision.gameObject.GetComponent<Health>().ChangeHealth(-20);
                UIManager.instance.UpdateHealth.Invoke(collision.gameObject.GetComponent<Health>());

            }




        }

    }




}


