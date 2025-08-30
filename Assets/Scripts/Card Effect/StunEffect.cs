using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Stun (Turn-Based)")]
public class StunEffect : CardEffect
{
    public int durationTurns = 1;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyStatus(new EnemyStatusEffect(StatusType.Stun, 0, durationTurns));
        }
    }
}
