using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Defense Reduction (Turn-Based)")]
public class DefenseReductionEffect : CardEffect
{
    public int reduceAmount = 5; 
    public int durationTurns = 2;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyStatus(new EnemyStatusEffect(StatusType.DefenseReduction, reduceAmount, durationTurns));
        }
    }
}

