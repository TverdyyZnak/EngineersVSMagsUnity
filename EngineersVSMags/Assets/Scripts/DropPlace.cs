using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DropDrag card = eventData.pointerDrag.GetComponent<DropDrag>();

        if (card) 
        {
            card.parentCardHolder = transform;
        }
    }
}
