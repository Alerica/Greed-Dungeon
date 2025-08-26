using System;
using System.Collections.Generic;

public class Player {
    public int HP;
    public int MaxHP;
    public int Energy = 5;
    public List<Card> Hand;

    public void PlayCard(Card card, Enemy target) {
        if (Energy >= card.Cost) {
            Energy -= card.Cost;
            card.Effect(this, target);
        }
    }

    internal void DrawCards(int v)
    {
        throw new NotImplementedException();
    }
}
