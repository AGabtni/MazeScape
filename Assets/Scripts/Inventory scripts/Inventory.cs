using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
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

    Dictionary<Ammo,int> ammunition = new Dictionary<Ammo, int>();

    public bool Add(Item item)
    {
        
        if(item.category!= Category.Ammo && itemsList.Count >= maxSpace) {


            Debug.Log("No space available");

            return false;


        }

        if(item.category == Category.Ammo){
            bool added = false;
            foreach(var pair in ammunition){
                if(pair.Key.Equals(item))
                    ammunition[pair.Key] += item.Amount;
                    added = true;
                    break;
                
                


            }
            if(!added)
                ammunition.Add((Ammo)item, item.Amount);
            
        }
        else
            itemsList.Add(item);

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
