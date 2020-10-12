using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class InventoryUI : MonoBehaviour
{

    public Transform itemsParent;
    [SerializeField]private GameObject inventoryWindow;


    
    #region Singleton
    public static InventoryUI instance;

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("There is more than one inventory UI instance ");

            return;
        }


        instance = this;
    }


    #endregion

    InventorySlot[] slots;

    private void Start()
    {
        //Subscribe the ui update function the on item changed callback
        Inventory.instance.onItemChangedCallback += UpdateUI;




        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        inventoryWindow.SetActive(false);


        UpdateUI();
    }


    public void OnInventoryBtnClicked()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);

    }


    private void Update()
    {




    }


    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.itemsList.Count)
            {

                slots[i].AddItem(Inventory.instance.itemsList[i]);

            }
            else
            {
                //Otherwise 
                slots[i].ClearSlot();
            }

        }
    }



}
