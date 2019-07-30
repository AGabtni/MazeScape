using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyMovement))]
public class NPCLogic : MonoBehaviour
{
    EnemyMovement movementController;
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
                Vector3 targetPosition = movementController.visibleTargets[i].position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);


                if (Vector3.Distance(movementController.visibleTargets[i].position,transform.position) > distance)
                {
                    movementController.SetDestination(movementController.visibleTargets[i].position);


                }
                else
                {
                    movementController.ResetDestination();
                }
            }
        }
        else
        {

            movementController.ResetDestination();
        }

        
    }
}
