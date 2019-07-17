using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour
{
    public float radius = 1.5f;
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * radius, Color.yellow);

        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("Fire 3");

            Collider[] hitColliders =
            Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hitCollider in hitColliders)
            {


                if(hitCollider.name == "Door") {

                    if (! hitCollider.transform.parent.parent.GetComponent<MazeDoor>().isOpen)
                    {
                        hitCollider.transform.parent.parent.SendMessage("OnPlayerEntered",
                            SendMessageOptions.DontRequireReceiver);

                    }
                    
                }   

            }
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}