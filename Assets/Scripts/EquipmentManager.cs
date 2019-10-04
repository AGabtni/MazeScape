using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;
    public MeshRenderer targetMesh;
    public GameObject targetHand;

    void Awake()
    {

        instance = this;
       
    }
    #endregion




    Weapon currentWeapon;

    // Callback for when an item is equipped/unequipped
    //public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    //public OnEquipmentChanged onEquipmentChanged;


    // Callback for when an item is equipped/unequipped
    public delegate void OnWeaponChanged(Weapon newWeapon, Weapon oldWeapon);
    public OnWeaponChanged onWeaponChanged;

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
        Transform weapon = Instantiate(currentWeapon.weaponPrefab, targetHand.transform) as Transform;
        weapon.localPosition = currentWeapon.PickUp_Position;
        weapon.localEulerAngles = currentWeapon.PickUp_Rotation;
    }

    public Weapon UnequipWeapon()
    {
        Weapon oldWeapon = null;

        if(currentWeapon != null) {

            oldWeapon = currentWeapon;
            inventory.Add(oldWeapon);


            currentWeapon = null;

            if(onWeaponChanged != null)
            {
                onWeaponChanged.Invoke(null, oldWeapon);
            }
    




        }


        return oldWeapon;

    }


}

public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }

