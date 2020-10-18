using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("There is more than one inventory instance ");

            return;
        }


        instance = this;
    }


    #endregion

    //Callback for adding/removing item
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;


    public int maxSpace = 15;

    public List<Item> itemsList = new List<Item>();

    public Dictionary<Ammo, int> ammunition = new Dictionary<Ammo, int>();

    public bool Add(Item item)
    {

        if (item.category != Category.Ammo && itemsList.Count >= maxSpace)
        {

            Debug.Log("No space available");
            return false;

        }

        if (item.category == Category.Ammo)
        {
            KeyValuePair<Ammo, int> foundAmmo;
            foreach (var pair in ammunition)
            {
                if (pair.Key.itemName.Equals(item.itemName))
                {
                    foundAmmo = pair;
                    break;
                }

            }
            if (foundAmmo.Key != null)
            {
                ammunition[foundAmmo.Key] += item.amount;
            }
            else
                ammunition.Add((Ammo)item, item.amount);


            //If weapon is equipped and needs ammo : 
            if (EquipmentManager.instance.weaponInstance != null)
                EquipmentManager.instance.weaponInstance.GetComponent<FireWeapon>().GetAmmo();

        }
        else if (item.category == Category.Weapon)
        {
            if(itemsList.Count == 0)
               item.Equip();
               
            itemsList.Add(item);

        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();


        return true;

    }

    public void Remove(Item item)
    {

        itemsList.Remove(item);


        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }


}
