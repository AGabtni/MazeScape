using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float interactionRadius = 1f;
    public LayerMask interactiveAgents;

    bool isFocus = false;
    bool hasInteracted = false;
    public bool canInteract = false;
    Transform agent;

    public virtual void Interact()
    {

    }



    private void FixedUpdate()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius,interactiveAgents);

        if (colliders.Length > 0)
        {
            Debug.Log("Player near by ");
            canInteract = true;
            //Here put a ui shit to show that item is nearby
        }
        else
        {

            canInteract = false;
        }


    }


    public void OnFocused (Transform agentTransform)
    {


        isFocus = true;
        hasInteracted = false;
        agent = agentTransform;

        Debug.Log("This item is focused on");

    }

    public void OnDefocused(Transform agentTransform)
    {


        isFocus = false;
        hasInteracted = false;
        agent = null;
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
