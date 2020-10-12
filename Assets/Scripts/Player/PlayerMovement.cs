using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform equipmentParent = null;

    [SerializeField] private float interactionRadius = 20.0f;
    [SerializeField] private float rotSpeed = 5.0f;
    [SerializeField] private float moveSpeed = 6.0f;

    [SerializeField] private float jumpSpeed = 10.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float terminalVelocity = -10.0f;
    [SerializeField] private float minFall = -1.5f;


    private CharacterController _charController;
    private CameraFollow _camController;
    private float _vertSpeed;
    private ControllerColliderHit _contact;

    private Animator _animator;
    private MazeCell _currentCell;


    [SerializeField] private bool ikActive = false;



    //IK animation members
    [SerializeField] private Transform lookObj = null;



    private VariableJoystick variableJoystick;
    private int raycastLayers;
    void Start()
    {
        int playerLayerMask = LayerMask.NameToLayer("Player");
        raycastLayers = (1 << playerLayerMask);
        raycastLayers = ~raycastLayers;


        _charController = GetComponent<CharacterController>();
        _camController = playerCamera.parent.GetComponent<CameraFollow>();

        _animator = GetComponent<Animator>();
        if (equipmentParent != null)
        {

            EquipmentManager.instance.equipmentParent = equipmentParent;

        }


#if UNITY_ANDROID && UNITY_EDITOR
        variableJoystick = GameObject.Find("Movement Joystick").GetComponent<VariableJoystick>();

#endif
    }

    Vector3 movement;
    void Update()
    {
        //TODO : Should be in different script . Since ultimate goal is to make this available on both mobile and desktop .
        #region  User Input Handler    


        //Movement Input based on platform
#if UNITY_ANDROID && !UNITY_EDITOR


        float horInput = variableJoystick.Horizontal;
        float vertInput = variableJoystick.Vertical;
#else

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

#endif


        //1- Movement command
        if (horInput != 0 || vertInput != 0)
        {
            movement = Vector3.zero;
            //Run movement
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            //TODO : fix backward  walking in aim mode
            if (playerCamera.parent.GetComponent<CameraFollow>().focus)
            {
                movement.z = Mathf.Clamp(movement.z, 0, Mathf.Abs(movement.z));
            }
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = playerCamera.rotation;
            playerCamera.eulerAngles = new Vector3(0, playerCamera.eulerAngles.y, 0);
            movement = playerCamera.TransformDirection(movement);
            playerCamera.rotation = tmp;
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);



        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);



        //2- Jump command
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
            _vertSpeed += gravity * 2 * Time.deltaTime;


        }


        //3- Aim command 
        if (Input.GetButtonDown("Aim"))
        {
            playerCamera.parent.GetComponent<CameraFollow>().CameraFocus(movement);
            _animator.SetBool("Aiming", !_animator.GetBool("Aiming"));
            toggleAnimationLayers(1);
            toggleAnimationLayers(0);

        }




        //4- Pickup/Interact command
#if UNITY_ANDROID
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = target.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);
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
#else
        if (Input.GetButtonDown("Interact"))
        {


            // TODO : Needs optimization for now just cast a sphere to see if any are interractable .
            //        Player must be able to choose the weapons to pickup.
            // Sol: Clickable ToolTip on weapon and trigger it if interactble is hit . 
            //      Player must click on tooltip to pickup . 
            //      Implement it from Interactable base class for All interactables
            RaycastHit[] hits = Physics.SphereCastAll(playerCamera.position, 5.0f, Vector3.forward, Mathf.Infinity, raycastLayers);
            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Collision with object : " + hit.transform.tag);
                if (hit.transform.GetComponent<Interactable>())
                {
                    Interactable interactable = hit.transform.GetComponent<Interactable>();
                    //Debug.Log("Found interactable");

                    if (interactable.canInteract)
                    {
                        interactable.Interact();
                        //Debug.Log("Has interacted");
                    }

                }
            }
        }
#endif




        //5- Inventory Command
        if (Input.GetButtonDown("Inventory"))
        {
            InventoryUI.instance.OnInventoryBtnClicked();
            _camController.TriggerCameraLock();
        }

        //6-Fire Command :
        if (Input.GetButtonDown("Fire1") && playerCamera.parent.GetComponent<CameraFollow>().focus )
        {
            EquipmentManager.instance.weaponSlot.OnSlotClicked();
        }

        #endregion
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);
    }


    void OnDrawGizmosSelected()
    {

        //Gizmos.DrawSphere(transform.position, interactionRadius);



    }

    //Store collision to use in Update
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }



    public void SetLocation(MazeCell cell)
    {
        if (_currentCell != null)
        {
            _currentCell.OnPlayerExited();
        }
        _currentCell = cell;
        transform.localPosition = cell.transform.localPosition;
        _currentCell.OnPlayerEntered();
    }


    //Switch between animations in Aim mode and in Normal between
    private void toggleAnimationLayers(int index)
    {

        if (_animator.GetLayerWeight(index) == 1)
        {
            _animator.SetLayerWeight(index, 0);
        }

        else if (_animator.GetLayerWeight(index) == 0)
        {
            _animator.SetLayerWeight(index, 1);
        }

    }

    //TODO : Needs testing and debugging before using it : 
    //IK animation
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
            if (equipmentParent != null)
            {
                //_animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);


                _animator.SetIKPosition(AvatarIKGoal.RightHand, equipmentParent.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, equipmentParent.rotation);

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


