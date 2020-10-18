using UnityEngine;

public class UserInput : MonoBehaviour
{
    public bool TEST;
    #region Singleton


    public static UserInput instance;

    void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("There is more than one User Input instance ");

            return;
        }


        instance = this;

    }

    #endregion

    private PlayerController _playerController;
    public PlayerController playerController
    {
        get { return _playerController; }
        set { _playerController = value; }
    }

    void Start()
    {
        //ONLY FOR WHITEBOXING
        if (TEST)
            playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {

        #region  User Input Handler    

        //Movement Input based on platform
#if UNITY_ANDROID && !UNITY_EDITOR

        float horInput = variableJoystick.Horizontal;
        float vertInput = variableJoystick.Vertical;
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            playerController.InteractMobile();
#else

        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Interact"))
            playerController.Interact();

#endif
        //1- Move Command
        if (horInput != 0 || vertInput != 0)
            playerController.MoveStep(horInput, vertInput);

        //2- Jump Command
        if (Input.GetButtonDown("Jump"))
            playerController.Jump();

        //3- Aim command 
        else if (Input.GetButtonDown("Aim"))
        {
            playerController.Aim();

        }

        //4- Fire command
        else if (Input.GetButtonDown("Fire1"))
        {
            if (playerController.isAiming)
                EquipmentManager.instance.weaponSlot.OnSlotClicked();
        }

        //5- Weapon Reload Command
        else if (Input.GetButtonDown("LoadAmmo"))
        {
            if(EquipmentManager.instance.weaponInstance.GetComponent<FireWeapon>())
                EquipmentManager.instance.weaponInstance.GetComponent<FireWeapon>().GetAmmo();
        }


        //6- Inventory Command
        else if (Input.GetButtonDown("Inventory"))
            playerController.TriggerInventory();

        //6- Pause Command
        else if (Input.GetButtonDown("Pause"))
            UIManager.instance.ToggleGameMenu();



        #endregion


    }

}