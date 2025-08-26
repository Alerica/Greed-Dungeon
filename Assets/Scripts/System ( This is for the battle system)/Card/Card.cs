using System;

public class Card {
    public string Name;
    public int Cost;
    public CardType Type;
    public Action<Player, Enemy> Effect; 
}