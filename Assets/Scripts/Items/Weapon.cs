using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Gun", menuName ="Items/Weapons/")]

public class Weapon : Item
{



    public enum WeaponCategory
    {
        Weapon,
        Consumable,
        Equipment
    }

    //Grenade, hand-to-hand, range, etc .... 
    public WeaponCategory weaponCategory;




    public int damageModifier;
    public Vector3 PickUp_Position;
    public Vector3 PickUp_Rotation;

    public Transform weaponPrefab;


    public override void Use()
    {

        base.Use();


        EquipmentManager.instance.EquipWeapon(this);
        RemoveFromInventory();


    }
    

    
}
