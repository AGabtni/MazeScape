using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public int id;
    public string itemName;



    //CATEGORIES CAN BE (WEAPON , ARMOR, CONSUMABLE , KEYITEM )
    public Item_Category category;
    
    public Sprite icon;






    public Dictionary<string, int> stats = new Dictionary<string, int>();
    public enum Item_Category
    {

        Weapon,
        Ammunition,
        Armor,
        Consumable,
        KeyItem

    };





    public Item(int id, Item_Category category, string itemName, Dictionary<string, int> stats)
    {

        this.id = id;
        this.category = category;
        this.itemName = itemName;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + itemName);
        this.stats = stats;

        

    }



    public Item(Item item)
    {

        this.id = item.id;
        this.category = item.category;

        this.itemName = item.itemName;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.itemName);
        this.stats = item.stats;



    }

}
