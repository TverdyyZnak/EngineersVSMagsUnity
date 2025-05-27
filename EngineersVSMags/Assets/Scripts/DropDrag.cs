using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Camera MainCamera;
    public Transform parentCardHolder;
    public Transform onBattleCards;
    public GameObject obj;
    private GameManager manager;
    private void Awake()
    {
        MainCamera = Camera.allCameras[0];
        manager = FindFirstObjectByType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.localScale = new Vector3((float)1.5, (float)1.5, 0);
        
        parentCardHolder = transform.parent;
        onBattleCards = transform.parent;
        transform.SetParent(parentCardHolder.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos = MainCamera.ScreenToWorldPoint(eventData.position);
        newPos.z = -10;
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        transform.SetParent(parentCardHolder);

        if (parentCardHolder == onBattleCards)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else 
        {
            for (int i = manager.game.PlayerHand.Count - 1; i >= 0; i--)
            {
                Card card = manager.game.PlayerHand[i];
                if (card.name == obj.GetComponent<CardInfo>().SelfCard.name)
                {
                    manager.game.PlayerAttac.Add(card);
                    manager.game.PlayerHand.RemoveAt(i);
                    
                    // Тута ману уменьшаю
                    manager.manaTxt.text = $"{manager.tempMana - Convert.ToInt32(obj.GetComponent<CardInfo>().mana.text)}";
                    manager.tempMana -= Convert.ToInt32(obj.GetComponent<CardInfo>().mana.text);


                    // Тута выключчаю кары с меньше
                    for (int j = 0; j < manager.playerHand.childCount; j++) 
                    {
                        if(Convert.ToInt32(manager.playerHand.GetChild(j).gameObject.GetComponent<CardInfo>().mana.text) > manager.tempMana) 
                        {
                            manager.playerHand.GetChild(j).GetComponent<DropDrag>().enabled = false;
                        }
                    }


                    break;
                }
            }
        }

        transform.localScale = new Vector3(1, 1, 0);


    }
}
