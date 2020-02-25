
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{   


    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {   
        originalPosition = transform.GetComponent<RectTransform>().position;
    }

    public void OnDrag(PointerEventData eventData){


        transform.position = Input.mousePosition;

    }  

    public void OnEndDrag(PointerEventData eventData)
    {
        
        transform.position = originalPosition;
    }
}
