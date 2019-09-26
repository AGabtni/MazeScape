using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Category itemCategory;
    public int Amount;


    public enum Category
    {
        Weapon, 
        Consumable,
        Equipment
    }


    
    public virtual void Use()
    {

        Debug.Log("Item"+itemName+" is used");
    }

    public virtual void PickUp()
    {

        Debug.Log("Item " + itemName + "is picked up");


    }

}
