using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour
{
    public float radius = 1.5f;
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            Debug.Log("Fire 3");

            Collider[] hitColliders =
            Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hitCollider in hitColliders)
            {


                if(hitCollider.name == "Door") {
                    Debug.Log(hitCollider.transform.parent.parent.name);

                    hitCollider.transform.parent.parent.SendMessage("OnPlayerEntered",
                    SendMessageOptions.DontRequireReceiver);
                }   

            }
        }
    }
}