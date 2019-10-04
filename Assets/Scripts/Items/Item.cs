using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{

    public Category slotCategory;
    public string itemName;
    public Sprite icon;
    public int Amount;


    public enum Category
    {
        Weapon, 
        Consumable,
        Equipment
    }


    
    public virtual void Use() { }


    public void RemoveFromInventory() {

        Inventory.instance.Remove(this);

    }






}
