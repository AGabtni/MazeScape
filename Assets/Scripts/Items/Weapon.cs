using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Gun", menuName ="Items/Weapons/")]

public class Weapon : Item
{



    public enum WeaponCategory
    {
        H2H,
        range,
        area
    }

    //Grenade, hand-to-hand, range, etc .... 
    public WeaponCategory weaponCategory;




    public int damageModifier;
    public Vector3 PickUp_Position;
    public Vector3 PickUp_Rotation;

    public Transform weaponPrefab;


    public override void Equip()
    {

        base.Equip();


        EquipmentManager.instance.EquipWeapon(this);
        RemoveFromInventory();


    }

    public override void UnEquip()
    {
        base.UnEquip();
        EquipmentManager.instance.UnequipWeapon();

    }
    public override void Use(){
        base.Use();



    }


    
}
