using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData cardData;
    public Card(CardData cardData)
    {
        this.cardData = cardData;
        EnergyCost = cardData.energyCost;
        BaseValue = cardData.baseValue;
        Effect = cardData.effects;

    }

    public Sprite Sprite => cardData.artwork;
    public string Name => cardData.cardName;
    public string Description => cardData.description;
    public CardType Type => cardData.cardType;
    public int EnergyCost { get; set; }
    public int BaseValue { get; set; }

    public List<CardEffect> Effect { get; set; }

    public void PerformEffect(GameObject target)
    {
        foreach (var effect in Effect)
        {
            Debug.Log($"Performing effect: {effect} with cost of {EnergyCost}" );
            effect.Apply(target); // Replace null with actual target when implementing
        }
    }
    
}
