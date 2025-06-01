using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardAttack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CardInfo playerCard;
    public CardInfo enemyCard;
    public GameManager gameManager;
    public Image image;
    private void Awake()
    {
        gameManager = Camera.allCameras[0].GetComponent<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("playerCard.SelfCard.name");
        Debug.Log(GetComponent<CardInfo>().SelfCard.name);
        if (gameManager.game.PlayerAttac.Contains(playerCard.SelfCard) && playerCard.isAttacked) 
        {
            if (gameManager.targetObj != null) 
            {
                gameManager.targetObj.GetComponent<CanvasGroup>().GetComponentInChildren<Image>().color = Color.white;
                gameManager.targetObj.isAtcTarget = false;
            }


            gameManager.targetObj = playerCard;
            gameManager.targetObj.GetComponent<CanvasGroup>().GetComponentInChildren<Image>().color = Color.green;
            gameManager.targetObj.isAtcTarget = true;

            Debug.Log($"Установил, крч {gameManager.targetObj.SelfCard.name}");
        }
        else if (gameManager.game.EnemyAttac.Contains(GetComponent<CardInfo>().SelfCard) && gameManager.targetObj != null) 
        {
            enemyCard = GetComponent<CardInfo>();

            Debug.Log($"Огонь по блядскому хутару, БЭЭЭЭУН {enemyCard.SelfCard.name}");

            //playerCard.isAttacked = false;

            gameManager.CardFight(enemyCard);
            
            gameManager.targetObj = null;

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.game.PlayerAttac.Contains(playerCard.SelfCard) && playerCard.isAttacked) 
        {
            image = GetComponent<CanvasGroup>().GetComponentInChildren<Image>();
            image.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (gameManager.game.PlayerAttac.Contains(playerCard.SelfCard) && playerCard.isAttacked)
        {
            image = GetComponent<CanvasGroup>().GetComponentInChildren<Image>();
            image.enabled = false;
        }

    }
}
