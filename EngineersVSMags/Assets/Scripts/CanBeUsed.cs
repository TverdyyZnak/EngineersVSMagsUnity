using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanBeUsed : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    public CardInfo cardInfo;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = Camera.allCameras[0].GetComponent<GameManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardInfo.isCanAttack && gameManager.game.PlayerHand.Contains(cardInfo.SelfCard) && gameManager.ReturnTurn() % 2 == 0) 
        {
            image = GetComponent<CanvasGroup>().GetComponentInChildren<Image>();
            image.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardInfo.isCanAttack) 
        {
            image = GetComponent<CanvasGroup>().GetComponentInChildren<Image>();
            image.enabled = false;
        }
        
    }
}
