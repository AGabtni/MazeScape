using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    List<Item> characterItems = new List<Item>();
    public GameObject Hand;
    private ItemsDatabase itemsDatabase;
    private GameObject inventoryPanel;
    private Button[] inventorySlots;
    private int freeSlot = 0;

    


    private void Start()
    {
        inventoryPanel = GameObject.Find("inventory_panel");
        inventorySlots = inventoryPanel.GetComponentsInChildren<Button>();
        itemsDatabase = FindObjectOfType<ItemsDatabase>();

        ClearSlots();
    }

    private void Update()
    {
      

    }
    public void TakeItem(int id)
    {
        

        
        Item itemToAdd = itemsDatabase.GetItem(0);
        inventorySlots[freeSlot].transform.GetChild(0).GetComponent<Image>().sprite = itemToAdd.icon;


        GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/Guns/"+itemToAdd.name);
        SkinnedMeshRenderer itemInstance = Instantiate<SkinnedMeshRenderer>(itemPrefab.GetComponent<SkinnedMeshRenderer>());
        itemInstance.transform.position = Hand.transform.position;
        itemInstance.transform.rotation = Hand.transform.localRotation;

        itemInstance.transform.parent = Hand.transform;

        characterItems.Add(itemToAdd);

        


        if(freeSlot<inventorySlots.Length)
            freeSlot++;
    }


    public void ClearSlots()
    {

        for(int i=0; i< inventorySlots.Length;  i++)
        {
            inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;


        }



    }



    public void EquipFromInventory()
    {



    }
}
