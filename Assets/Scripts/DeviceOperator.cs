using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour
{
    public float distanceToDoor = 1.5f;
    public LayerMask deviceMask;
    void Update()
    {
      
        RaycastHit hit;

        #if UNITY_EDITOR
       
        if (Input.GetButtonDown("Fire3"))
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distanceToDoor, deviceMask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                if (hit.collider.name == "Door")
                {

                    if (!hit.transform.parent.parent.GetComponent<MazeDoor>().isDoorOpen)
                    {
                        hit.transform.parent.parent.SendMessage("OnPlayerEntered",
                            SendMessageOptions.DontRequireReceiver);

                    }
                }

            }



        }
        #elif UNITY_ANDROID && !UNITY_EDITOR

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    Vector3 touchPosWorld = GetComponentInChildren<Camera>().ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (Physics.Raycast(touchPosWorld, GetComponentInChildren<Camera>().transform.forward, out hit, deviceMask) )
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        if (hit.collider.name == "Door")
                        {

                            if (!hit.transform.parent.parent.GetComponent<MazeDoor>().isDoorOpen)
                            {
                                hit.transform.parent.parent.SendMessage("OnPlayerEntered",
                                    SendMessageOptions.DontRequireReceiver);

                            }
                        }

                    }



                }

        #endif
    }



}