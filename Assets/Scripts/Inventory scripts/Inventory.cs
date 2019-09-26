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

    
    public bool Add(Item item)
    {

        if(itemsList.Count >= maxSpace) {


            Debug.Log("No space available");

            return false;


        }

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
