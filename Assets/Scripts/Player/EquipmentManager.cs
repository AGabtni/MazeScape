using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;
    //public MeshRenderer targetMesh;



    void Awake()
    {

        instance = this;

    }

    #endregion





    // Callback for when armor/clothes/style items are equipped/unequipped
    //public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    //public OnEquipmentChanged onEquipmentChanged;


    // Callback for when an item is equipped/unequipped
    public delegate void OnWeaponChanged(Weapon newWeapon, Weapon oldWeapon);
    public OnWeaponChanged onWeaponChanged;


    public EquipmentSlot weaponSlot;
    private Transform _weaponInstance;
    public Transform weaponInstance
    {
        get { return _weaponInstance; }
        private set { _weaponInstance = value; }
    }

    private Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get { return _currentWeapon; }
        private set { _currentWeapon = value; }
    }

    Inventory inventory;	// Reference to our inventory

    private void Start()
    {
        inventory = Inventory.instance;

    }


    public void EquipWeapon(Weapon newWeapon)
    {
        Weapon oldWeapon = UnequipWeapon();

        if (onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newWeapon, oldWeapon);
        }

        //Spawn weapon
        currentWeapon = newWeapon;

        weaponInstance = Instantiate(currentWeapon.itemPrefab) as Transform;

        PlayerController _controller = FindObjectOfType<PlayerController>();
        if (_controller.isAiming)
            _controller.OnWeaponActive();
        else
            _controller.OnWeaponInactive();
            

        //Remove weapons pickup component
        GameObject.Destroy(weaponInstance.GetComponent<ItemPickup>());
        //Add weapon to an equipment slot
        weaponSlot.AddItem(currentWeapon);

    }



    public Weapon UnequipWeapon()
    {
        Weapon oldWeapon = null;

        if (currentWeapon != null)
        {

            oldWeapon = currentWeapon;
            inventory.Add(oldWeapon);
            weaponSlot.ClearSlot();

            currentWeapon = null;
            if (weaponInstance != null)
            {
                Destroy(weaponInstance.GetComponent<FireWeapon>());
                Destroy(weaponInstance.gameObject);

            }

            if (onWeaponChanged != null)
            {

                onWeaponChanged.Invoke(null, oldWeapon);

            }






        }


        return oldWeapon;

    }

    //Should be weapon agnostic . 
    //  Needs to add Weapon Type Member to weapon scriptable item
    public void TriggerWeapon()
    {

        if (weaponInstance != null)
        {
            weaponInstance.GetComponent<FireWeapon>().Shoot();
        }
    }
    public void Update()
    {


    }

}
