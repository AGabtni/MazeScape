using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public List<Item> items;



    public string weaponsRep = "Prefabs/Weapon/Guns";
    public string throwableRep = "Prefabs/Weapon/Grenades";
    public string ammoRep = "Prefabs/Ammo";


    GameObject[] guns;
    GameObject[] grenades;
    GameObject[] ammo;

    int itemsNumber;
    private void Awake()
    {
        items = new List<Item>(); ;



        //Items repertories



        guns = Resources.LoadAll<GameObject>(weaponsRep);
        grenades = Resources.LoadAll<GameObject>(throwableRep);
        ammo = Resources.LoadAll<GameObject>(ammoRep);


        BuildDatabase();


        itemsNumber = guns.Length + grenades.Length + ammo.Length;

    }

    void BuildDatabase()
    {

        for (int i=0; i < guns.Length; i++)
        {

            Item newItem = new Item(
                                i,
                                Item.Item_Category.Weapon,
                                guns[i].name,
                                new Dictionary<string, int> {
                                    {"Defence",1 }
                                });
            items.Add(newItem);
        }


        for (int i = guns.Length; i < guns.Length + grenades.Length; i++)
        {

            Item newItem = new Item(
                                i,
                                Item.Item_Category.Weapon,
                                grenades[i-guns.Length].name,
                                new Dictionary<string, int> {
                                    {"Range",10 },
                                    {"Power",80 }
                                }
            );
            items.Add(newItem);
        }


        for (int i = guns.Length+grenades.Length; i < guns.Length + grenades.Length+ammo.Length; i++)
        {

            Item newItem = new Item(
                                i,
                                Item.Item_Category.Ammunition,
                                ammo[i-(guns.Length + grenades.Length)].name,
                                new Dictionary<string, int> {
                                    {"MaxAmount",99 },
                                    {"Power",5}
                                });
            items.Add(newItem);
        }

        Debug.Log(items[0].icon.name);
    }


    //Get item by its id
    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);


    }

    //Get item by its category
    public Item GetItem(Item.Item_Category category)
    {
        return items.Find(item => item.category == category);


    }

    //Get item by its name
    public Item GetItemByName(string name)
    {
        return items.Find(item => item.name == name);


    }
}
