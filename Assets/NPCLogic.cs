using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
    public Transform lookFrom;
    public float distance = 5.0f;
    private void Start()
    {
        movementController = GetComponent<EnemyMovement>();
    }




    private void Update()
    {


        if (movementController.visibleTargets.Count > 0)
        {
            
            for (int i = 0; i < movementController.visibleTargets.Count; i++)
            {
                transform.LookAt(movementController.visibleTargets[i]);
                if (Vector3.Distance(movementController.visibleTargets[i].position,transform.position) > distance)
                {
                    //transform.LookAt(movementController.visibleTargets[i].position);
                    //Debug.Log("fenfrj");
                    movementController.SetDestination(movementController.visibleTargets[i].position);


                }


                //Debug.Log("Target found number (" + i + ") is at : " + movementController.visibleTargets[i]);

            }
        }
        else
        {

            movementController.ResetDestination();
        }

        /*
         *         int layerMask = LayerMask.GetMask("Target");

        RaycastHit hit;

        Vector3 right = new Vector3(1, 0, 1);
        Vector3 left = new Vector3(-1, 0, 1);

        if (Physics.Raycast(lookFrom.position, transform.TransformDirection(Vector3.forward), out hit, distance,layerMask) ||
            Physics.Raycast(lookFrom.position, transform.TransformDirection(right), out hit, distance, layerMask) ||
            Physics.Raycast(lookFrom.position, transform.TransformDirection(left), out hit, distance, layerMask) 
            )
        {
            if(hit.distance>= 1.0f)
            {
                movementController.SetDestination(hit.transform.position);


            }
            else
            {
                //Debug.Log("Attack");
            }
        }
        else
        {
            Debug.DrawRay(lookFrom.position, transform.TransformDirection(Vector3.forward) * distance, Color.white);
            Debug.DrawRay(lookFrom.position, transform.TransformDirection(right) * distance, Color.white);
            Debug.DrawRay(lookFrom.position, transform.TransformDirection(left) * distance, Color.white);

            
        }
        */
    }
}
