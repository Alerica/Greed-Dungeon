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

    [Header("Visuals")]
    public GameObject vfxPrefab; // assign the prefab in inspector
}

public enum CardType
{
    Power,
    Buff,
    Utility
}
