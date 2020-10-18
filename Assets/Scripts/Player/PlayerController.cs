using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool IKTESTS;
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private Transform equipmentParent = null;
    [SerializeField] private Transform rightHand = null;

    [Header("IK animation game objects")]
    [SerializeField] private Transform lookObj = null;
    [SerializeField] private Transform rightHandObj = null;
    [SerializeField] private Transform leftHandObj = null;


    [Header("Movement and collision settings")]
    [SerializeField] private float interactionRadius = 20.0f;
    [SerializeField] private float rotSpeed = 6.0f;
    [SerializeField] private float moveSpeed = 6.0f;

    [SerializeField] private float jumpSpeed = 10.0f;
    [SerializeField] private float gravity = -9.8f;
    //[SerializeField] private float terminalVelocity = -10.0f;
    //[SerializeField] private float minFall = -1.5f;

    private MeshDestroy _meshController;
    private VariableJoystick variableJoystick;

    private Health _playerHealth;
    public Health playerHealth
    {
        get { return _playerHealth; }
        set { _playerHealth = value; }
    }
    private CharacterController _charController;
    private CameraFollow _camController;
    private float _vertSpeed;
    private ControllerColliderHit _contact;
    private MazeCell _currentCell;

    private Animator _animator;
    public Animator animator
    {
        get { return _animator; }
    }
    private bool _isAiming;
    public bool isAiming
    {
        get { return _isAiming; }
        set { _isAiming = value; }
    }
    private bool _isPlayerAlive = true;
    private int raycastLayers;
    [SerializeField] private bool ikActive = false;



    void Start()
    {
#if UNITY_ANDROID && UNITY_EDITOR
        variableJoystick = GameObject.Find("Movement Joystick").GetComponent<VariableJoystick>();

#endif
        _meshController = GetComponentInChildren<MeshDestroy>();

        int playerLayerMask = LayerMask.NameToLayer("Player");
        raycastLayers = 1 << playerLayerMask;
        raycastLayers = ~raycastLayers;

        _charController = GetComponent<CharacterController>();
        _camController = playerCamera.parent.GetComponent<CameraFollow>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<Health>();
        _playerHealth.healthFill = GameObject.Find("HealthFill").GetComponent<Image>();

        isAiming = false;


    }

    Vector3 movement;
    public void MoveStep(float horInput, float vertInput)
    {

        movement = Vector3.zero;
        //Run movement
        movement.x = horInput * moveSpeed;
        movement.z = vertInput * moveSpeed;
        movement = Vector3.ClampMagnitude(movement, moveSpeed);

        //If is not aiming change player's rotation and movement to face the camera's direction
        if (!isAiming)
        {

            //Remove X component from camera rotation before making movement direction match the camera's
            Quaternion tmp = playerCamera.rotation;
            playerCamera.eulerAngles = new Vector3(0, playerCamera.eulerAngles.y, 0);
            movement = playerCamera.TransformDirection(movement);
            playerCamera.rotation = tmp;

            //Rotate player towards camera
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }


    }

    public void Jump()
    {
        if (_charController.isGrounded)
        {

            _vertSpeed = jumpSpeed;
            _animator.SetBool("Jumping", true);

        }

    }

    public void Interact()
    {
        // TODO : Needs optimization for now just cast a sphere to see if any are interractable .
        //        Player must be able to choose the weapons to pickup.
        // Sol: Clickable ToolTip on weapon and trigger it if interactble is hit . 
        //      Player must click on tooltip to pickup . 
        //      Implement it from Interactable base class for All interactables
        Collider[] hits = Physics.OverlapSphere(playerCamera.position, 2.5f, raycastLayers);
        foreach (Collider collider in hits)
        {
            Debug.Log("Collision with object : " + collider.transform.tag);
            if (collider.transform.GetComponent<Interactable>())
            {
                Interactable interactable = collider.transform.GetComponent<Interactable>();
                //Debug.Log("Found interactable");

                if (interactable.canInteract)
                {
                    interactable.Interact();
                    //Debug.Log("Has interacted");
                }

            }
        }

    }

    public void InteractMobile()
    {

        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
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

    public void Aim()
    {


        isAiming = !isAiming;
        ikActive = !ikActive;
        UIManager.instance.UpdateHUD.Invoke(isAiming);
        if (EquipmentManager.instance.weaponInstance != null)
        {
            if (isAiming)
            {
                OnWeaponActive();
            }
            else
                OnWeaponInactive();
        }
        _animator.SetBool("Aiming", !_animator.GetBool("Aiming"));
        toggleAnimationLayers(1);
        toggleAnimationLayers(0);
    }

    public void TriggerInventory()
    {
        InventoryUI.instance.OnInventoryBtnClicked();
        _camController.TriggerCameraLock();
    }

    float rotY;
    float rotX;
    void Update()
    {
        if (!playerHealth.IsAlive())
        {
            OnDeath();
            return;
        }

        if (isAiming)
        {


            //Change player rotation to face camera's direction
            Quaternion tmp = playerCamera.rotation;
            playerCamera.eulerAngles = new Vector3(0, playerCamera.eulerAngles.y, 0);
            movement = playerCamera.TransformDirection(movement);
            playerCamera.rotation = tmp;

            Quaternion direction = Quaternion.LookRotation(playerCamera.forward);
            direction.eulerAngles = new Vector3(0, direction.eulerAngles.y + 10.0f, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);

            //Rotate equipment to cameras direction
            Quaternion gunDirection = Quaternion.LookRotation(playerCamera.forward);
            lookObj.rotation = Quaternion.Lerp(lookObj.rotation, gunDirection, rotSpeed * Time.deltaTime);


        }
        _vertSpeed += gravity * 2 * Time.deltaTime;
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _charController.Move(movement);

        if (!_charController.isGrounded)
            _animator.SetBool("Jumping", false);
        _animator.SetFloat("Speed", _charController.velocity.sqrMagnitude);






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

    //IK animation for gun aiming
    void OnAnimatorIK()
    {


        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive || IKTESTS)
        {

            // Set the look target position, if one has been assigned
            if (lookObj != null)
            {
                _animator.SetLookAtWeight(1);
                _animator.SetLookAtPosition(lookObj.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (rightHandObj != null && leftHandObj != null)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);



                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
            }

        }

        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetLookAtWeight(0);
        }

    }

    public void OnWeaponActive()
    {
        EquipmentManager.instance.weaponInstance.SetParent(equipmentParent);
        EquipmentManager.instance.weaponInstance.localPosition = Vector3.zero;
        EquipmentManager.instance.weaponInstance.localEulerAngles = Vector3.zero;
    }

    public void OnWeaponInactive()
    {
        EquipmentManager.instance.weaponInstance.SetParent(rightHand);
        EquipmentManager.instance.weaponInstance.localPosition = EquipmentManager.instance.currentWeapon.PickUp_Position;
        EquipmentManager.instance.weaponInstance.localEulerAngles = EquipmentManager.instance.currentWeapon.PickUp_Rotation;
    }

    public void OnDeath()
    {
        if (!_isPlayerAlive)
            return;

        _isPlayerAlive = false;
        _meshController.DestroyMesh();
        _charController.isTrigger = true;
        UIManager.instance.ToggleGameMenu(true);
    }

    //Store collision to use in Update
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }

}


