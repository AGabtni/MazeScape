using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform target = null;

    public Transform inventoryPanel;
    public Camera playerCamera;


    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;
    private ControllerColliderHit _contact;

    private Animator _animator;
    private MazeCell currentCell;

    private MazeDirection currentDirection;

    public bool ikActive = false;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform lookObj = null;

    public LayerMask groundExcludeMask;
    private CharacterController _charController;



    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        #if UNITY_ANDROID && !UNITY_EDITOR
            variableJoystick = GameObject.Find("Movement Joystick").GetComponent<VariableJoystick>();

        #endif
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;

        
        //Input based on platform
        #if UNITY_ANDROID && !UNITY_EDITOR


            float horInput = variableJoystick.Horizontal;
            float vertInput = variableJoystick.Vertical;
        #elif UNITY_EDITOR

            float horInput = Input.GetAxis("Horizontal");
            float vertInput = Input.GetAxis("Vertical");
        
        #endif



        if (horInput != 0 || vertInput != 0)
        {
            //Run movement
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation,
                direction, rotSpeed * Time.deltaTime);

          
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);


       


        // y movement: possibly jump impulse up, always accel down
        // could _charController.isGrounded instead, but then cannot workaround dropoff edge
        if (_charController.isGrounded)
        {

            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
                GetComponent<PlayerHealth>().RestoreHealth();
                _animator.SetBool("Jumping", true);

            }
            else
            {
                _vertSpeed = 0;
            }
        }
        else
        {
            _animator.SetBool("Jumping", false);
            _vertSpeed += gravity *5* Time.deltaTime;
          
           
        }
        movement.y = _vertSpeed;





        movement *= Time.deltaTime;
        _charController.Move(movement);

        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Interactable interactable = raycastHit.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.OnFocused(transform);
                }
            }
        }

    }

    // store collision to use in Update
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }

    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        currentCell.OnPlayerEntered();
    }



    //IK animation : 

    private void OnAnimatorIK()
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {

            // Set the look target position, if one has been assigned
            if (lookObj != null)
            {
                _animator.SetLookAtWeight(1);
                _animator.SetLookAtPosition(lookObj.position);

            }

            // Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null)
            {
                //_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

               
                _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                
            }
            

        }

        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetLookAtWeight(0);
        }
    }
}


