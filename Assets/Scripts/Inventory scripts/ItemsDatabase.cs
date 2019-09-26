using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public List<Item> items;
    
    int itemsNumber;
    private void Awake()
    {
        items = new List<Item>(); ;



        //Items repertories





        BuildDatabase();



    }

    void BuildDatabase()
    {




    }


 

    //Get item by its name
    public Item GetItemByName(string name)
    {
        return items.Find(item => item.name == name);


    }
}
