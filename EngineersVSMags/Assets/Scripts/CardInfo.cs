using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
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

        hp.text = card.hp.ToString();
        dmg.text = card.dmg.ToString();
        mana.text = card.mana.ToString();


    }

    private void Start()
    {
        //ShowCardInfo(CardCollection.CardsIng[transform.GetSiblingIndex()]);
    }
}
