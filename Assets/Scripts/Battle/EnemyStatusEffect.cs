using UnityEngine;

[System.Serializable]
public class EnemyStatusEffect
{
    public StatusType type;
    public int value;        // Damage, defense reduction, etc.
    public int turnsRemaining;

    public EnemyStatusEffect(StatusType type, int value, int turns)
    {
        this.type = type;
        this.value = value;
        this.turnsRemaining = turns;
    }
}

public enum StatusType
{
    Burn,
    Frost,
    Leech,
    Bleed,
    Stun,
    Soak,
    DefenseReduction,

    Gnawed
}