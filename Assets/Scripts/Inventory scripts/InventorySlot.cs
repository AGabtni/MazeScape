using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour
{

    public Item item;
    public Button removeButton;


    private Image Icon;



    public void Awake()
    {

        Icon = transform.Find("Icon").GetComponent<Image>();


    }

    public void OnSlotClicked()
    {



        if (item != null )
        {
            if(EquipmentManager.instance.weaponInstance != null)
                EquipmentManager.instance.UnequipWeapon();
            item.Equip();
        }


        
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        

        Icon.sprite = item.icon;
        Icon.enabled = true;
        
        
       
       
        
        //removeButton.interactable = true;

        
    }



    public void ClearSlot()
    {


        item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        //removeButton.interactable = false;

    }
  
}
