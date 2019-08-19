using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public float interactionRadius = 1f;


    bool isFocus = false;
    bool hasInteracted = false;

    Transform agent;


    private void Update()
    {
        if (isFocus)
        {

            float distance = Vector3.Distance(agent.position, transform.position);
            if(distance<= interactionRadius)
            {

                Debug.Log("Pick me up ui &/or effect");
                hasInteracted = true;


            }
        }
    }


    public void OnFocused (Transform agentTransform)
    {


        isFocus = true;
        agent = agentTransform;
    }

    public void OnDefocused(Transform agentTransform)
    {


        isFocus = false;
        agent = null;
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
