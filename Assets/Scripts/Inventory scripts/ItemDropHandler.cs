
using UnityEngine;

using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {



        RectTransform invPanel = transform.GetComponent<RectTransform>();


        if(!RectTransformUtility.RectangleContainsScreenPoint(invPanel,Input.mousePosition)){

            eventData.selectedObject.GetComponent<EquipmentSlot>().item.UnEquip();
    

        }
    }

}
