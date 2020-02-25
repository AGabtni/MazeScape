using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentSlot : MonoBehaviour
{

    public Item item;
    public Button removeButton;

    private Text Amount;
    private Image Icon;

    public Category slot_category;
    public void Awake()
    {

        Amount = transform.Find("Amount").GetComponent<Text>();
        Icon = transform.Find("Icon").GetComponent<Image>();


    }


      public void AddItem(Item newItem)
    {
        item = newItem;


        if(item.slotCategory == Category.Equipment)
        {
            Amount.gameObject.SetActive(false);

        }
        Icon.sprite = item.icon;
        Icon.enabled = true;
        //removeButton.interactable = true;

        
    }



    public void ClearSlot()
    {


        item = null;

        Amount.gameObject.SetActive(false);

        Icon.sprite = null;
        Icon.enabled = false;
        //removeButton.interactable = false;

    }


    public void OnSlotClicked(){

        if(item!=null){

            item.UnEquip();
        }

    }
}