using UnityEngine;

public class Card
{
    private readonly CardData cardData;
    public Card(CardData cardData)
    {
        this.cardData = cardData;
        EnergyCost = cardData.energyCost;
        BaseValue = cardData.baseValue;
        Effect = cardData.Effect;

    }

    public Sprite Sprite => cardData.artwork;
    public string Name => cardData.cardName;
    public string Description => cardData.description;
    public CardType Type => cardData.cardType;
    public int EnergyCost { get; set; }
    public int BaseValue { get; set; }

    public string Effect { get; set; }

    public void PerformEffect()
    {
        Debug.Log($"Performing effect: {Effect} with cost of {EnergyCost}" );
    }
    
}
