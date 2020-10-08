using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;
    //public MeshRenderer targetMesh;
    public Transform targetHand;

    Weapon currentWeapon;
    




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



    public Transform weaponInstance;

    public EquipmentSlot weaponSlot;

    Inventory inventory;	// Reference to our inventory

    private void Start()
    {
        inventory = Inventory.instance;
        weaponSlot.ClearSlot();
        
    }


    public void EquipWeapon (Weapon newWeapon)
    {
        Weapon oldWeapon = UnequipWeapon();

        if(onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newWeapon, oldWeapon);
        }

        //Spawn weapon
        currentWeapon = newWeapon;
        weaponInstance = Instantiate(currentWeapon.itemPrefab, targetHand) as Transform;
        GameObject.Destroy(weaponInstance.GetComponent<ItemPickup>());
        weaponInstance.GetComponent<WeaponController>().enabled = true;
        weaponInstance.localPosition = currentWeapon.PickUp_Position;
        weaponInstance.localEulerAngles = currentWeapon.PickUp_Rotation;

        //Add it to equipment slot
        weaponSlot.AddItem(currentWeapon);  


        
            if(weaponSlot.onItemUsedCallback != null)
                weaponSlot.onItemUsedCallback.Invoke();  
    }

    public Weapon UnequipWeapon()
    {
        Weapon oldWeapon = null;

        if(currentWeapon != null) {

            oldWeapon = currentWeapon;
            inventory.Add(oldWeapon);
            weaponSlot.ClearSlot();   

            currentWeapon = null;
            if(weaponInstance != null)
            {
                Destroy(weaponInstance.GetComponent<WeaponController>());
                Destroy(weaponInstance.gameObject);

            }

            if (onWeaponChanged != null)
            {

                onWeaponChanged.Invoke(null, oldWeapon);
                
            }

            
            



        }


        return oldWeapon;

    }

    public void Update(){

        
    }

}
