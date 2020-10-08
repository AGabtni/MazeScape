using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum Category
    {
        Weapon, 
        Consumable,
        Equipment,
        Ammo
    }

public class Item : ScriptableObject
{

    public Category category;
    public string itemName;
    public Sprite icon;
    public int amount = 0 ;
    public int maxAmount;

    public Transform itemPrefab ;

    public virtual void Equip(){
        
    }

    public virtual void UnEquip(){
        
    }
    
    public virtual void Use() {
        

    }


    public void RemoveFromInventory() {

        Inventory.instance.Remove(this);

    }






}
