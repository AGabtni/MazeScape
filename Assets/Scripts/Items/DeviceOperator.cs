using UnityEngine;

public class DeviceOperator : Interactable
{

    public override void Interact()
    {
        base.Interact();

        interactWithDoor();
    }

    void interactWithDoor()
    {
        Debug.Log("Interact with door");
        if (!transform.GetComponent<MazeDoor>().isDoorOpen)
        {
            transform.GetComponent<MazeDoor>().OnPlayerEntered();
        }





    }



}