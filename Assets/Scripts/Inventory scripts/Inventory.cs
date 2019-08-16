using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    List<Item> characterItems = new List<Item>();
    private ItemsDatabase itemsDatabase;
    private GameObject inventoryPanel;
    private void Awake()
    {
        inventoryPanel = GameObject.Find("inventory_panel");        
    }
    public void TakeItem(int id)
    {


        Item itemToAdd = itemsDatabase.GetItem(id);
        characterItems.Add(itemToAdd);

        inventoryPanel.GetComponentInChildren<Image>();




    }
}
