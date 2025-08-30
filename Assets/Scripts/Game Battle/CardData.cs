using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/New Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int energyCost;
    public CardType cardType;
    public int baseValue;
    public List<CardEffect> effects; 
}

public enum CardType
{
    Power,
    Buff,
    Utility
}
