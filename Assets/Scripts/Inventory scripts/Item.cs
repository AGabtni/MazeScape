using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int id;
    public string category;
    public string name;
    
    public Sprite icon;



    public Dictionary<string, int> stats = new Dictionary<string, int>();



    public Item(int id, string category, string name, Dictionary<string, int> stats)
    {

        this.id = id;
        this.category = category;
        this.name = name;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + name);

        this.stats = stats;

        

    }



    public Item(Item item)
    {

        this.id = item.id;
        this.category = item.category;

        this.name = item.name;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.name);

        this.stats = item.stats;



    }

}
