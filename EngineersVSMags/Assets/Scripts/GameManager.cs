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
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using UnityEngine.SceneManagement;

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

    public List<Card> GiveDeckCard(string s)
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
    public string playerF;
    public string enemyF;

    public GameObject magsWin;
    public GameObject engWin;

    public bool isPlayerTurn
    {
        get
        {
            return turn % 2 == 0;
        }
    }

    public CardInfo targetObj = null;
    public Game game;
    public Transform playerHand;
    public Transform playerAttac;
    public Transform enemyHand;
    public GameObject cadrPrefab;
    public NetworkManager network;
    public string EnemyCardsNum;

    public TextMeshProUGUI manaTxt;
    public int tempMana = 1;
    private int constantMana;

    public TextMeshProUGUI playerHP;
    public TextMeshProUGUI enemyHP;
    private int constantHP = 20;

    private bool messageReceived = false;
    private string messageCache = "";

    private int turn;
    private int turnTime = 30;

    public TextMeshProUGUI timeTxt;
    public Button endTurn;

    public CardInfo yourTarget;

    public bool battleSocketisActive = false;

    private void Awake()
    {
        playerF = UnionClass.plF;
        enemyF = UnionClass.emF;
    }
    private void Start()
    {
        if (network.isServer)
        {
            constantMana = 1;
            turn = 2;
        }
        else
        {
            constantMana = 0;
            turn = 3;
            endTurn.interactable = false;
        }

        game = new Game(playerF, enemyF);


        playerHP.text = constantHP.ToString();
        enemyHP.text = constantHP.ToString();


        GiveHandCards(game.GiveDeckCard(playerF), game.PlayerHand, playerHand);

        if (turn % 2 != 0)
        {
            for (int i = 0; i < playerHand.childCount; i++)
            {
                playerHand.GetChild(i).gameObject.GetComponent<DropDrag>().enabled = false;
            }
        }

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
            network.messageFrom = ""; // Ó˜Ë˘‡ÂÏ Ò‡ÁÛ
        }

        if (messageReceived)
        {
            messageReceived = false;
            HandleMessage(messageCache);
        }

        if (Convert.ToInt32(enemyHP.text) <= 0)
        {
            Debug.Log("“€ œŒ¡≈ƒ»À!!!");
            if (enemyF == "ING")
            {
                magsWin.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                engWin.GetComponent<SpriteRenderer>().enabled = true;
            }


            Invoke("Exit", 5f);
        }
        else if (Convert.ToInt32(playerHP.text) <= 0)
        {
            Debug.Log("“€ œ–Œ»√–¿À(");
            if (playerF == "ING")
            {
                magsWin.GetComponent<SpriteRenderer>().enabled = true;
            }
            else 
            {
                engWin.GetComponent <SpriteRenderer>().enabled = true;
            }


            Invoke("Exit", 5f);
        }


        if (Input.GetKeyDown(KeyCode.P)) 
        {
            Application.Quit();
        }
    }

    private void Exit() 
    {
        network.NetworkExit();
        SceneManager.LoadScene(0);
    }

    public int ReturnTurn() 
    {
        return turn;
    }

    public void GuardCheck() 
    {
        int dmg = 0;
        string msg = "";

        if(enemyHand.childCount == 0) 
        {
            for(int i = 0; i < playerAttac.childCount; i++) 
            {
                if (playerAttac.GetChild(i).gameObject.GetComponent<CardInfo>().isAttacked) 
                {
                    dmg += playerAttac.GetChild(i).gameObject.GetComponent<CardInfo>().SelfCard.dmg;
                }
            }
        }

        Debug.Log($"”ÓÂ: {dmg}");

        if (dmg > 0) 
        {
            enemyHP.text = (Convert.ToInt32(enemyHP.text) - dmg).ToString();
            msg += "D;";
            msg += playerHand.childCount.ToString() + ";";
            msg += dmg.ToString() + ";";
            network.SendTcpMessage(msg);
        }

    }

    public void WhoWin() 
    {
        if(Convert.ToInt32(enemyHP.text) <= 0) 
        {
            string msg = "You lost!";
            Debug.Log(msg);
        }
    }

    void HandleMessage(string message)
    {
        string msgType = message[0].ToString();
        EnemyCardsNum = message[2].ToString();

        if(msgType == "L") 
        {
        }

        if (msgType == "W") 
        {
        }

        if(msgType == "D")
        {
            Debug.Log("œÓÎÛ˜ÂÌËÂ ÛÓÂ‡");

            string temp = "";

            for(int i = 4; i < message.Length; i++) 
            {
                if(message[i].ToString() == ";") 
                {
                    if (temp != "") 
                    {
                        playerHP.text = (Convert.ToInt32(playerHP.text) - Convert.ToInt32(temp)).ToString();
                    }
                }
                else 
                {
                    temp += message[i].ToString();
                }
            }
        }

        if (msgType == "T")
        {


            enemyHand.DetachChildren();
            playerAttac.DetachChildren();

            // “”“ ƒŒ¡¿¬À≈Õ»≈  ¿–“€   VVV

            Card card = null;

            int iTemp = 6;
            string temp = "";

            while (message[iTemp].ToString() != "Y")
            {
                if (message[iTemp].ToString() == ".")
                {
                    foreach (Card item in game.GiveDeckCard(enemyF))
                    {
                        if (temp == item.id)
                        {
                            card = item;
                            break;
                        }
                    }

                    temp = "";
                }
                else if (message[iTemp].ToString() == ";")
                {
                    cadrPrefab.GetComponent<DropDrag>().enabled = false;

                    cadrPrefab.GetComponent<CardInfo>().isCanAttack = false;

                    game.EnemyAttac.Add(card);
                    GameObject cardGO = Instantiate(cadrPrefab, enemyHand, false);
                    cardGO.GetComponent<CardInfo>().ShowCardInfo(card);
                    cardGO.GetComponent<CardInfo>().hp.text = temp;


                    temp = "";
                }
                else
                {
                    temp += message[iTemp].ToString();
                }


                iTemp++;
            }

            iTemp += 2;
            temp = "";

            Debug.Log(iTemp);
            card = null;

            for (int i = iTemp; i < message.Length; i++)
            {
                if (message[i].ToString() == ".")
                {
                    foreach (Card item in game.GiveDeckCard(playerF))
                    {
                        if (temp == item.id)
                        {
                            card = item;
                            break;
                        }
                    }

                    temp = "";
                }
                else if (message[i].ToString() == ";")
                {
                    cadrPrefab.GetComponent<DropDrag>().enabled = false;
                    cadrPrefab.GetComponent<CardInfo>().isCanAttack = false;

                    game.PlayerAttac.Add(card);
                    GameObject cardGO = Instantiate(cadrPrefab, playerAttac, false);
                    cardGO.GetComponent<CardInfo>().ShowCardInfo(card);
                    cardGO.GetComponent<CardInfo>().hp.text = temp;

                    card = null;
                    temp = "";
                }
                else
                {
                    temp += message[i].ToString();
                }
            }

            temp = "";

            // “”“ ƒŒ¡¿¬À≈Õ»≈  ¿–“€   AAA
            message = "";

            turn++;

            StopAllCoroutines();
            StartCoroutine(turnFun());
            constantMana++;
            tempMana = constantMana;
            manaTxt.text = tempMana.ToString();
            endTurn.interactable = turn % 2 == 0;

            for(int j  = 0; j < playerAttac.childCount; j++) 
            {
                playerAttac.GetChild(j).gameObject.GetComponent<CardAttack>().enabled = true;
            }

            //¬˚ÍÎ˛˜‡˛ Í‡Ú˚, Ì‡ ÍÓÚÓ˚Â ÌÂ ı‚‡Ú‡ÂÚ Ï‡Ì˚

            for (int z = 0; z < playerHand.childCount; z++)
            {
                playerHand.GetChild(z).gameObject.GetComponent<DropDrag>().enabled = true;
                playerHand.GetChild(z).gameObject.GetComponent<CardInfo>().isCanAttack = true;

                if (Convert.ToInt32(playerHand.GetChild(z).gameObject.GetComponent<CardInfo>().mana.text) > tempMana)
                {
                    playerHand.GetChild(z).gameObject.GetComponent<DropDrag>().enabled = false;
                    playerHand.GetChild(z).gameObject.GetComponent<CardInfo>().isCanAttack = false;
                }
            }

            for (int z = 0; z < playerAttac.childCount; z++)
            {
                playerAttac.GetChild(z).gameObject.GetComponent<CardInfo>().isAttacked = true;
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

            for (int j = 0; j < playerHand.childCount; j++)
            {
                playerHand.GetChild(j).GetComponent<DropDrag>().enabled = false;   
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

        if (handCollection.Count < 7)
        {
            handCollection.Add(dack[a]);
            Card card = dack[a];

            if(card.mana > 1) 
            {
                cadrPrefab.GetComponent<DropDrag>().enabled = false;
                cadrPrefab.GetComponent<CardInfo>().isCanAttack = false;
            }
            else
            {
                cadrPrefab.GetComponent<DropDrag>().enabled = true;
                cadrPrefab.GetComponent<CardInfo>().isCanAttack = true;
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

            GuardCheck();

            string msg = "";

            msg += "T;";
            msg += game.PlayerHand.Count.ToString() + ';';
            msg += "E;";

            for(int i = 0; i < playerAttac.childCount; i++) 
            {
                msg += playerAttac.GetChild(i).GetComponent<CardInfo>().SelfCard.id + '.';
                msg += playerAttac.GetChild(i).GetComponent<CardInfo>().hp.text + ';';
            }

            msg += "Y;";

            for (int i = 0; i < enemyHand.childCount; i++) 
            {
                msg += enemyHand.GetChild(i).GetComponent<CardInfo>().SelfCard.id + '.';
                msg += enemyHand.GetChild(i).GetComponent<CardInfo>().hp.text + ';';
            }


            Debug.Log(msg);

            for (int i = 0; i < playerHand.childCount; i++) 
            {
                playerHand.GetChild(i).GetComponent<DropDrag>().enabled = false;
            }

            for (int i = 0; i < playerAttac.childCount; i++) 
            {
                playerAttac.GetChild(i).GetComponent<CardAttack>().enabled = false;
            }

            network.SendTcpMessage(msg);
        }


        StartCoroutine(turnFun());
    }

    public void CardFight(CardInfo enemy) 
    {

        
        Debug.Log($"{targetObj.hp.text} + {enemy.hp.text}");


        

        targetObj.hp.text = (Convert.ToInt32(targetObj.hp.text) - enemy.SelfCard.dmg).ToString();              
        enemy.hp.text = (Convert.ToInt32(enemy.hp.text) - targetObj.SelfCard.dmg).ToString();

        Debug.Log($"{targetObj.hp.text} + {enemy.hp.text}");

        targetObj.isAttacked = false;

        if (Convert.ToInt32(targetObj.hp.text) <= 0) 
        {
            game.PlayerAttac.Remove(targetObj.SelfCard);
            game.PlayerDeck.Remove(targetObj.SelfCard);
            Destroy(targetObj.gameObject);
        }

        if(Convert.ToInt32(enemy.hp.text) <= 0)
        {
            game.EnemyAttac.Remove(enemy.SelfCard);
            game.EnemyDeck.Remove(enemy.SelfCard);
            Destroy(enemy.gameObject);
        }
    }
}
