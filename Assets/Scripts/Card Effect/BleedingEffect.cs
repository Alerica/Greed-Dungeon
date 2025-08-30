using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Bleeding (Turn-Based)")]
public class BleedEffect : CardEffect
{
    public int damagePerTurn = 5;
    public int durationTurns = 3;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null) // flesh check
        {
            enemy.ApplyStatus(new EnemyStatusEffect(StatusType.Bleed, damagePerTurn, durationTurns));
        }
    }
}
