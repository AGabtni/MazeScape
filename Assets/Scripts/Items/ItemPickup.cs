using UnityEngine;
using System.Collections;

public class ItemPickup : Interactable
{
    public Item item;


    public override void Interact()
    {
        base.Interact();

        PickUp();

        Debug.Log(item.itemName + " Picked up");
    }


    void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp)
        {

            if (item.category == Category.Weapon)
            {   

                
                Ammo newAmmo = new Ammo();
                newAmmo.weapon = (Weapon)item;
                newAmmo.itemName = item.itemName + "Ammo";
                newAmmo.amount = item.amount;
                newAmmo.category = Category.Ammo;
                Inventory.instance.Add(newAmmo);
            }
            //Here destroyed but ideally it would be
            //Pooled up from the catalog/object pooler

            Destroy(gameObject);



        }




    }
}
