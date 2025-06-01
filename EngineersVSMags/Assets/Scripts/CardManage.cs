using System.Collections.Generic;
using UnityEngine;


public class Card
{
    public string id;
    public string name;
    public Sprite image;
    public int hp;
    public int dmg;
    public int mana; 

    public Card() 
    {

    }

    public Card(string id, string name, Sprite imagePath, int hp, int dmg, int mana)
    {
        this.id = id;
        this.name = name;
        this.image = imagePath;
        this.hp = hp;
        this.dmg = dmg;
        this.mana = mana;
    }

    public Card(string id, string name, string imagePath, int hp, int dmg, int mana)
    {
        this.id = id;
        this.name = name;
        this.image = Resources.Load<Sprite>(imagePath);
        this.hp = hp;
        this.dmg = dmg;
        this.mana = mana;
    }
}

public static class CardCollection
{
    public static List<Card> CardsIng = new List<Card>();
    public static List<Card> CardsMag = new List<Card>();
}


public class CardManage : MonoBehaviour
{
    private void Awake()
    {
        //Инженеры

        CardCollection.CardsIng.Add(new Card("E1", "Miner", "Sprites/IngCards/1", 2, 3, 2));
        CardCollection.CardsIng.Add(new Card("E1", "Miner", "Sprites/IngCards/1", 2, 3, 2));
        CardCollection.CardsIng.Add(new Card("E1", "Miner", "Sprites/IngCards/1", 2, 3, 2));
        CardCollection.CardsIng.Add(new Card("E1", "Miner", "Sprites/IngCards/1", 2, 3, 2));
        
        CardCollection.CardsIng.Add(new Card("E2", "Electrician", "Sprites/IngCards/2", 1, 1, 1));
        CardCollection.CardsIng.Add(new Card("E2", "Electrician", "Sprites/IngCards/2", 1, 1, 1));
        CardCollection.CardsIng.Add(new Card("E2", "Electrician", "Sprites/IngCards/2", 1, 1, 1));
        CardCollection.CardsIng.Add(new Card("E2", "Electrician", "Sprites/IngCards/2", 1, 1, 1));
        
        CardCollection.CardsIng.Add(new Card("E3", "Old Master", "Sprites/IngCards/3", 2, 4, 3));
        CardCollection.CardsIng.Add(new Card("E3", "Old Master", "Sprites/IngCards/3", 2, 4, 3));
        CardCollection.CardsIng.Add(new Card("E3", "Old Master", "Sprites/IngCards/3", 2, 4, 3));
        CardCollection.CardsIng.Add(new Card("E4", "Sniper", "Sprites/IngCards/4", 1, 5, 2));
        
        CardCollection.CardsIng.Add(new Card("E4", "Sniper", "Sprites/IngCards/4", 1, 5, 2));
        CardCollection.CardsIng.Add(new Card("E5", "Tank", "Sprites/IngCards/5", 5, 5, 7));
        CardCollection.CardsIng.Add(new Card("E5", "Tank", "Sprites/IngCards/5", 5, 5, 7));
        CardCollection.CardsIng.Add(new Card("E6", "LaserMan", "Sprites/IngCards/6", 2, 5, 4));
        
        CardCollection.CardsIng.Add(new Card("E6", "LaserMan", "Sprites/IngCards/6", 2, 5, 4));
        CardCollection.CardsIng.Add(new Card("E7", "Oreshnik", "Sprites/IngCards/7", 3, 15, 9));
        CardCollection.CardsIng.Add(new Card("E8", "Juggernaut", "Sprites/IngCards/8", 10, 1, 6));
        CardCollection.CardsIng.Add(new Card("E8", "Juggernaut", "Sprites/IngCards/8", 10, 1, 6));
        
        CardCollection.CardsIng.Add(new Card("E9", "Hacker", "Sprites/IngCards/9", 2, 1, 1));
        CardCollection.CardsIng.Add(new Card("E9", "Hacker", "Sprites/IngCards/9", 2, 1, 1));
        CardCollection.CardsIng.Add(new Card("E10", "Jetpack", "Sprites/IngCards/10", 3, 5, 5));
        CardCollection.CardsIng.Add(new Card("E10", "Jetpack", "Sprites/IngCards/10", 3, 5, 5));

        //Маги

        CardCollection.CardsMag.Add(new Card("M1", "Neon Ninja", "Sprites/MagsCards/1", 5, 3, 3));
        CardCollection.CardsMag.Add(new Card("M1", "Neon Ninja", "Sprites/MagsCards/1", 5, 3, 3));
        CardCollection.CardsMag.Add(new Card("M2", "Zap Mag", "Sprites/MagsCards/2", 2, 2, 2));
        CardCollection.CardsMag.Add(new Card("M2", "Zap Mag", "Sprites/MagsCards/2", 2, 2, 2));

        CardCollection.CardsMag.Add(new Card("M2", "Zap Mag", "Sprites/MagsCards/2", 2, 2, 2));
        CardCollection.CardsMag.Add(new Card("M3", "Fire Mag", "Sprites/MagsCards/3", 3, 4, 3));
        CardCollection.CardsMag.Add(new Card("M3", "Fire Mag", "Sprites/MagsCards/3", 3, 4, 3));
        CardCollection.CardsMag.Add(new Card("M4", "Light Spirit", "Sprites/MagsCards/4", 1, 2, 1));

        CardCollection.CardsMag.Add(new Card("M4", "Light Spirit", "Sprites/MagsCards/4", 1, 2, 1));
        CardCollection.CardsMag.Add(new Card("M4", "Light Spirit", "Sprites/MagsCards/4", 1, 2, 1));
        CardCollection.CardsMag.Add(new Card("M4", "Light Spirit", "Sprites/MagsCards/4", 1, 2, 1));
        CardCollection.CardsMag.Add(new Card("M4", "Light Spirit", "Sprites/MagsCards/4", 1, 2, 1));

        CardCollection.CardsMag.Add(new Card("M5", "Magic Wall", "Sprites/MagsCards/5", 15, 0, 6));
        CardCollection.CardsMag.Add(new Card("M5", "Magic Wall", "Sprites/MagsCards/5", 15, 0, 6));
        CardCollection.CardsMag.Add(new Card("M5", "Magic Wall", "Sprites/MagsCards/5", 15, 0, 6));
        CardCollection.CardsMag.Add(new Card("M6", "JoJo", "Sprites/MagsCards/6", 7, 11, 9));

        CardCollection.CardsMag.Add(new Card("M7", "Skeleton", "Sprites/MagsCards/7", 2, 1, 1));
        CardCollection.CardsMag.Add(new Card("M7", "Skeleton", "Sprites/MagsCards/7", 2, 1, 1));
        CardCollection.CardsMag.Add(new Card("M8", "Harry Potter", "Sprites/MagsCards/8", 5, 4, 4));
        CardCollection.CardsMag.Add(new Card("M8", "Harry Potter", "Sprites/MagsCards/8", 5, 4, 4));

        CardCollection.CardsMag.Add(new Card("M9", "Elf Archer", "Sprites/MagsCards/9", 3, 5, 5));
        CardCollection.CardsMag.Add(new Card("M9", "Elf Archer", "Sprites/MagsCards/9", 3, 5, 5));
        CardCollection.CardsMag.Add(new Card("M10", "Monk", "Sprites/MagsCards/10", 7, 8, 8));
        CardCollection.CardsMag.Add(new Card("M10", "Monk", "Sprites/MagsCards/10", 7, 8, 8));

    }


}
