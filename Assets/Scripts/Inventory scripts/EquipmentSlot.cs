using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentSlot : MonoBehaviour
{   

    //Callback for using item
    public delegate void OnItemUsed();
    public OnItemUsed onItemUsedCallback;

    public Item item;
    public Button removeButton;

    private Text Amount;
    private Image Icon;

    public Category slot_category;
    public void Awake()
    {
        if(slot_category != Category.Equipment)
            Amount = transform.Find("Amount").GetComponent<Text>();

        Icon = transform.Find("Icon").GetComponent<Image>();
        
    }


      public void AddItem(Item newItem)
    {
        item = newItem;

        if(slot_category != Category.Equipment)
            Amount.gameObject.SetActive(true);
            
        Amount.text = ""+newItem.Amount;
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

            item.Use();
            Amount.text = ""+item.Amount;

        }

    }   
}