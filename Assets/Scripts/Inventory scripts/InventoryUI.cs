using UnityEngine;
using System.Collections;

public class InventoryUI : MonoBehaviour
{

    public Transform itemsParent;
    public GameObject inventoryUI;


    Inventory inventory; // current player inventory


    InventorySlot[] slots;

    private void Start()
    {
        inventory = Inventory.instance;
        //Subscribe the ui update function the on item changed callback
        inventory.onItemChangedCallback += UpdateUI;




        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventoryUI.SetActive(false);


    }



    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
            inventoryUI.SetActive(!inventoryUI.activeSelf);


    }


    private void UpdateUI()
    {
        for (int i=0; i< slots.Length; i++)
        {
            if( i < inventory.itemsList.Count)
            {

                slots[i].AddItem(inventory.itemsList[i]);
            }
            else
            {
                //Otherwise 
                slots[i].ClearSlot();

            }

        }
    }



}
