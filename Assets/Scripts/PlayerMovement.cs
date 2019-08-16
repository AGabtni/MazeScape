using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private VariableJoystick variableJoystick;
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


        // raycast down to address steep slopes and dropoff edge
        bool hitGround = false;
        RaycastHit hit;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (_charController.height + _charController.radius) / 1.9f;
            hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
        }

        // y movement: possibly jump impulse up, always accel down
        // could _charController.isGrounded instead, but then cannot workaround dropoff edge
        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
                GetComponent<PlayerHealth>().RestoreHealth();
            }
            else
            {
                _vertSpeed = minFall;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }
            if (_contact != null)
            {   // not right at level start
                _animator.SetBool("Jumping", true);
            }

            // workaround for standing on dropoff edge
            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;

        movement *= Time.deltaTime;
        _charController.Move(movement);
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
}

