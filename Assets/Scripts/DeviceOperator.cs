using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour
{
    public float radius = 1.5f;
    void Update()
    {
        int layerMask = 1 << 8;
        
        layerMask = ~layerMask;
        RaycastHit hit;

       
        if (Input.GetButtonDown("Fire3"))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, radius, layerMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.name == "Door")
                {

                    if (!hit.transform.parent.parent.GetComponent<MazeDoor>().isOpen)
                    {
                        hit.transform.parent.parent.SendMessage("OnPlayerEntered",
                            SendMessageOptions.DontRequireReceiver);

                    }
                }

            }
        
        }
    }


  
}