using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    public List<Item> items;

    GameObject[] guns;
    GameObject[] grenades;
    GameObject[] ammo;

    int itemsNumber;
    private void Awake()
    {
        items = new List<Item>(); ;

        guns = Resources.LoadAll<GameObject>("Prefabs/Guns");
        grenades = Resources.LoadAll<GameObject>("Prefabs/Grenades");
        ammo = Resources.LoadAll<GameObject>("Prefabs/Ammo");


        BuildDatabase();


        itemsNumber = guns.Length + grenades.Length + ammo.Length;

    }

    void BuildDatabase()
    {

        for (int i=0; i < guns.Length; i++)
        {

            Item newItem = new Item(
                                i,
                                "weapon",
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
                                "throwable",
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
                                ammo[i-(guns.Length + grenades.Length)].name,
                                "amunition",
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
    public Item GetItem(string category)
    {
        return items.Find(item => item.category == category);


    }

    //Get item by its name
    public Item GetItemByName(string name)
    {
        return items.Find(item => item.name == name);


    }
}
