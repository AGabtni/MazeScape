using UnityEngine;
using System.Collections;

public class ItemPickup : Interactable
{
    public Item item;


    public override void Interact()
    {
        base.Interact();

        PickUp();
    }


    void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(item);

        if (wasPickedUp )
        {


            //Here destroyed but ideally it would be
            //Pooled up from the catalog/object pooler

            Destroy(gameObject);



        }




    }
}
