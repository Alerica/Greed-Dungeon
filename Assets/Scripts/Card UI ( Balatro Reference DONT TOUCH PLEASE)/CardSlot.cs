using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public Card currentCard;

    public void SetCard(Card card)
    {
        currentCard = card;
        card.currentSlot = this; // link back
    }
}
