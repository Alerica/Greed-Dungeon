using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Frost (Turn-Based)")]
public class FrostEffect : CardEffect
{
    public int reduceAmount = 5; // amount of damage reduction
    public int durationTurns = 2;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyStatus(new EnemyStatusEffect(StatusType.Frost, reduceAmount, durationTurns));
        }
    }
}
