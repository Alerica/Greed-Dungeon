using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<CardData> allCardData;
    [SerializeField] private CardView cardView;
    private List<Card> playerDeck = new List<Card>();

    [SerializeField] private Transform handHolder;

    private void Start()
    {
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        Debug.Log("Initializing Deck");
        playerDeck = new();
        for (int i = 0; i < allCardData.Count; i++)
        {
            Debug.Log($"Card {i}: {allCardData[i].cardName}");
            CardData data = allCardData[Random.Range(0, allCardData.Count)];
            Card card = new Card(data);
            playerDeck.Add(card);
        }
    }

    public void DrawCard()
    {
        if (playerDeck.Count == 0)
        {
            Debug.Log("Deck is empty, cannot draw a card.");
            return;
        }
        
        Card drawnCard = playerDeck[Random.Range(0, playerDeck.Count)];
        playerDeck.Remove(drawnCard);
        CardView cardInstance = Instantiate(cardView, handHolder);
        cardInstance.Setup(drawnCard);
        handHolder.GetComponent<HandLayout>().RepositionCards();

        Debug.Log($"Drew card: {drawnCard.Name} - {drawnCard.Description}");
        Debug.Log($"Remaining cards in deck: {playerDeck.Count}");
    }
    
}
