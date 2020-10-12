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

    public Text Amount;
    private Image Icon;

    public Category slot_category;
    public void Start()
    {
        if (slot_category != Category.Equipment)
            Amount = transform.Find("Amount").GetComponent<Text>();

        Icon = transform.Find("Icon").GetComponent<Image>();

        onItemUsedCallback += UpdateSlot;

    }


    public void AddItem(Item newItem)
    {
        item = newItem;

        //removeButton.interactable = true;
        Amount.gameObject.SetActive(newItem.category == Category.Weapon ? true : false);
        Icon.sprite = item.icon;
        Icon.enabled = true;
        if (onItemUsedCallback != null)
            onItemUsedCallback.Invoke();

    }



    public void ClearSlot()
    {


        item = null;

        Amount.gameObject.SetActive(false);

        Icon.sprite = null;
        Icon.enabled = false;
        //removeButton.interactable = false;

    }

    void UpdateSlot()
    {

        if (EquipmentManager.instance.weaponInstance != null)
            Amount.text = "" + EquipmentManager.instance.weaponInstance.GetComponent<FireWeapon>().currentAmmo;


    }
    public void OnSlotClicked()
    {

        if (item != null)
        {

            item.Use();
            if (onItemUsedCallback != null)
                onItemUsedCallback.Invoke();

        }

    }
}