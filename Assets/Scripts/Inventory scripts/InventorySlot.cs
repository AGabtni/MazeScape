using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour
{

    public Item item;
    public Button removeButton;


    private Text Amount;
    private Image Icon;



    public void Awake()
    {

        Amount = transform.Find("Ammo").GetComponent<Text>();
        Icon = transform.Find("Icon").GetComponent<Image>();


    }

    public void OnSlotClicked()
    {
        if (item != null )
        {
            
            item.Equip();
        }
        
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        

        Icon.sprite = item.icon;
        Icon.enabled = true;
        Amount.text = ""+item.Amount ;
        Amount.gameObject.SetActive(true);
        if(item.category == Category.Equipment)
        {
            Amount.gameObject.SetActive(false);
            return;

        }
        
       
       
        
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
  
}
