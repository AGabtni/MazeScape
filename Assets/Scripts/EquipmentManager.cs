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

    Inventory inventory;	// Reference to our inventory


    private void Start()
    {
        inventory = Inventory.instance;
        
    }


    public void EquipWeapon (Weapon newWeapon)
    {
        Weapon oldWeapon = UnequipWeapon();

        if(onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newWeapon, oldWeapon);
        }


        currentWeapon = newWeapon;
        weaponInstance = Instantiate(currentWeapon.weaponPrefab, targetHand) as Transform;
        weaponInstance.localPosition = currentWeapon.PickUp_Position;
        weaponInstance.localEulerAngles = currentWeapon.PickUp_Rotation;
    }

    public Weapon UnequipWeapon()
    {
        Weapon oldWeapon = null;

        if(currentWeapon != null) {

            oldWeapon = currentWeapon;
            inventory.Add(oldWeapon);


            currentWeapon = null;
            if(weaponInstance != null)
            {
                Destroy(weaponInstance.gameObject);

            }

            if (onWeaponChanged != null)
            {

                onWeaponChanged.Invoke(null, oldWeapon);
                
            }
            
            



        }


        return oldWeapon;

    }


}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }

