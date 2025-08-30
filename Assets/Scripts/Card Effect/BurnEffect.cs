using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Burn (Turn-Based)")]
public class BurnEffect : CardEffect
{
    public int burnDamage = 5;
    public int durationTurns = 3;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyStatus(new StatusEffect(StatusType.Burn, burnDamage, durationTurns));
        }
    }
}
