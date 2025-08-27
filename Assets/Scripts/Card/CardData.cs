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

    public string Effect;
}

public enum CardType
{
    Power,
    Buff,
    Utility
}
