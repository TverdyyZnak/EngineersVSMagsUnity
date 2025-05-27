using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Threading;
using UnityEditor;
using System;

public class Game
{
    public List<Card> PlayerHand;
    public List<Card> EnemyHand;
    public List<Card> PlayerAttac;
    public List<Card> EnemyAttac;
    public List<Card> PlayerDeck;
    public List<Card> EnemyDeck;

    public string side;

    public Game(string side, string enemySide)
    {
        this.side = side;
        PlayerDeck = GiveDeckCard(side);
        EnemyDeck = GiveDeckCard(enemySide);

        PlayerHand = new List<Card>();
        EnemyHand = new List<Card>();

        PlayerAttac = new List<Card>();
        EnemyAttac = new List<Card>();

    }

    private List<Card> GiveDeckCard(string s)
    {
        if (s == "ING")
        {
            List<Card> localList = CardCollection.CardsIng
            .Select(c => new Card(c.id, c.name, c.image, c.hp, c.dmg, c.mana))
            .ToList();

            return localList;
        }
        else
        {
            List<Card> localList = CardCollection.CardsMag
            .Select(c => new Card(c.id, c.name, c.image, c.hp, c.dmg, c.mana))
            .ToList();

            return localList;
        }

    }

}

public class GameManager : MonoBehaviour
{

    public bool isPlayerTurn
    {
        get
        {
            return turn % 2 == 0;
        }
    }

    public Game game;
    public Transform playerHand;
    public Transform enemyHand;
    public GameObject cadrPrefab;
    public NetworkManager network;
    public string EnemyCardsNum;

    public TextMeshProUGUI manaTxt;
    public int tempMana = 1;
    private int constantMana = 1;

    private bool messageReceived = false;
    private string messageCache = "";

    private int turn;
    private int turnTime = 30;

    public TextMeshProUGUI timeTxt;
    public Button endTurn;


    private void Start()
    {
        if (network.isServer)
        {
            turn = 2;
        }
        else
        {
            turn = 3;
            endTurn.interactable = false;
        }
        game = new Game("MAG", "ING");



        GiveHandCards(game.PlayerDeck, game.PlayerHand, playerHand);

        if (network.isServer)
        {
            StartCoroutine(turnFun());
        }
        else
        {

        }


    }

    void Update()
    {
        if (!string.IsNullOrEmpty(network.messageFrom))
        {
            messageCache = network.messageFrom;
            messageReceived = true;
            network.messageFrom = ""; // очищаем сразу
        }

        if (messageReceived)
        {
            messageReceived = false;
            HandleMessage(messageCache);
        }
    }

    void HandleMessage(string message)
    {
        EnemyCardsNum = message[0].ToString();

        string temp = "";

        enemyHand.DetachChildren();

        for (int i = 1; i < message.Length; i++)
        {
            temp += message[i];
            if (i % 2 == 0)
            {
                Card card = new Card();

                Debug.Log(temp);
                Debug.Log(CardCollection.CardsIng.Count);

                foreach(Card item in CardCollection.CardsIng)
                {
                    if (temp == item.id)
                    {
                        Debug.Log(item);
                        card = item;
                        break;
                    }
                }

                Debug.Log(CardCollection.CardsIng.Count);
                Debug.Log(card.name);

                cadrPrefab.GetComponent<DropDrag>().enabled = false;
                GameObject cardGO = Instantiate(cadrPrefab, enemyHand, false);
                cardGO.GetComponent<CardInfo>().ShowCardInfo(card);



                temp = "";
            }
        }

        message = "";

        turn++;

        StopAllCoroutines();
        StartCoroutine(turnFun());
        constantMana++;
        tempMana = constantMana;
        manaTxt.text = tempMana.ToString();
        endTurn.interactable = turn % 2 == 0;


        //Выключаю карты, на которые не хватает маны

        for (int z = 0; z < playerHand.childCount; z++) 
        {
            playerHand.GetChild(z).gameObject.GetComponent<DropDrag>().enabled = true;
            if(Convert.ToInt32(playerHand.GetChild(z).gameObject.GetComponent<CardInfo>().mana.text) > tempMana) 
            {
                playerHand.GetChild(z).gameObject.GetComponent<DropDrag>().enabled = false;
            }

        }
    }

    IEnumerator turnFun()
    {
        turnTime = 50;
        timeTxt.text = turnTime.ToString();

        if (turn % 2 == 0)
        {
            while (turnTime-- > 0)
            {
                timeTxt.text = turnTime.ToString();
                yield return new WaitForSeconds(1);
            }
            
            ChangeTurn();
        }
        else
        {

        }

    }

    void GiveHandCards(List<Card> dack, List<Card> handCollection, Transform hand)
    {
        for (int i = 0; i < 4; i++)
        {
            GiveCardsToHand(dack, handCollection, hand);
        }
    }

    void GiveCardsToHand(List<Card> dack, List<Card> handCollection, Transform hand)
    {
        if (dack.Count == 0)
        {
            return;
        }

        int a = UnityEngine.Random.Range(0, dack.Count);

        if (handCollection.Count < 8)
        {
            handCollection.Add(dack[a]);
            Card card = dack[a];

            if(card.mana > 1) 
            {
                cadrPrefab.GetComponent<DropDrag>().enabled = false;
            }
            else
            {
                cadrPrefab.GetComponent<DropDrag>().enabled = true;
            }
            GameObject cardGO = Instantiate(cadrPrefab, hand, false);

            cardGO.GetComponent<CardInfo>().ShowCardInfo(card);

            dack.RemoveAt(a);
        }
    }


    public void ChangeTurn() //Finish
    {
        StopAllCoroutines();
        manaTxt.text = "0";
        turn++;
        endTurn.interactable = turn % 2 == 0;

        if (turn % 2 != 0)
        {
            endTurn.interactable = false;
            GiveCardsToHand(game.PlayerDeck, game.PlayerHand, playerHand);

            string msg = "";
            msg += game.PlayerHand.Count.ToString();

            foreach (Card card in game.PlayerAttac)
            {
                msg += card.id;
            }

            Debug.Log(msg);

            network.SendTcpMessage(msg);
        }

        StartCoroutine(turnFun());
    }
}
