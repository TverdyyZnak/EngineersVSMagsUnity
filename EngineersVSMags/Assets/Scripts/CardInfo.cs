using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
    public bool isCanAttack;
    public bool isAttacked = true;
    public bool isAtcTarget = false;

    public Card SelfCard;
    public Image Logo;
    public TextMeshProUGUI name;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI mana;

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        Logo.sprite = card.image;
        Logo.preserveAspect = true;
        name.text = card.name;

        hp.text = SelfCard.hp.ToString();
        dmg.text = SelfCard.dmg.ToString();
        mana.text = SelfCard.mana.ToString();
    }

    public void UpdateHp(string newHP) 
    {
        hp.text = SelfCard.hp.ToString();
        
    }

    private void Start()
    {
        //ShowCardInfo(CardCollection.CardsIng[transform.GetSiblingIndex()]);
    }
}
