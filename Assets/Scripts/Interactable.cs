using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float interactionRadius = 1f;
    public LayerMask interactiveAgents;

    bool isFocus = false;
    bool hasInteracted = false;

    Transform agent;

    public virtual void Interact()
    {

    }
    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius,interactiveAgents);

        if (colliders.Length > 0)
        {
            Debug.Log("Here");
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
