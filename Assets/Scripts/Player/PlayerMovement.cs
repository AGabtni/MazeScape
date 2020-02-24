using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{



    [SerializeField] private Transform target = null;

    private Camera playerCamera;


    public float rotSpeed = 5.0f;
    public float moveSpeed = 6.0f;

    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float _vertSpeed;
    private ControllerColliderHit _contact;

    private Animator _animator;
    private MazeCell currentCell;


    public bool ikActive = false;


    [SerializeField] private Transform rightHandObj = null;

    [SerializeField] private Transform lookObj = null;

    private CharacterController _charController;
    private VariableJoystick variableJoystick;



    void Start()
    {

        playerCamera = target.GetComponent<Camera>();
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if (rightHandObj != null)
        {

            EquipmentManager.instance.targetHand = rightHandObj;

        }


#if UNITY_ANDROID && UNITY_EDITOR
        variableJoystick = GameObject.Find("Movement Joystick").GetComponent<VariableJoystick>();

#endif
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;


        //Input based on platform
#if UNITY_ANDROID && UNITY_EDITOR


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
            //move camera to face player direction only when its not aiming
            if (!target.parent.GetComponent<CameraFollow>().focus)
            {
                Quaternion tmp = target.rotation;
                target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
                movement = target.TransformDirection(movement);
                target.rotation = tmp;
                Debug.Log("cam");
                Quaternion direction = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Lerp(transform.rotation,
                direction, rotSpeed * Time.deltaTime);
            }else{


                transform.rotation =  Quaternion.LookRotation(movement);
            }

            


        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);





        // y movement: possibly jump impulse up, always accel down
        // could _charController.isGrounded instead, but then cannot workaround dropoff edge
        if (_charController.isGrounded)
        {

            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
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
            _vertSpeed += gravity * 5 * Time.deltaTime;


        }
        movement.y = _vertSpeed;



        if (Input.GetButtonDown("Aim"))
        {
            target.parent.GetComponent<CameraFollow>().CameraFocus(movement);
            
            _animator.SetBool("Aiming", !_animator.GetBool("Aiming"));
                        toggleAnimationLayers(1);

            toggleAnimationLayers(0);

        }

        movement *= Time.deltaTime;
        _charController.Move(movement);


        //Pickup object if touched 
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = playerCamera.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                Debug.Log(raycastHit.transform.tag);

                if (raycastHit.transform.GetComponent<Interactable>())
                {
                    Interactable interactable = raycastHit.transform.GetComponent<Interactable>();
                    Debug.Log("Found interactable");

                    if (interactable.canInteract)
                    {
                        interactable.Interact();
                        Debug.Log("Has interacted");
                    }
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


    //
    private void toggleAnimationLayers(int index ){

        if(_animator.GetLayerWeight(index) == 1 ){
            _animator.SetLayerWeight(index, 0 );
            Debug.Log(_animator.GetLayerIndex("Normal mode"));
        }

        else if(_animator.GetLayerWeight(index) == 0 ){
            _animator.SetLayerWeight(index, 1 );
        }
        
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


