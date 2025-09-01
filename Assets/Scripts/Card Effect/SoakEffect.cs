using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Soak (Turn-Based)")]
public class SoakEffect : CardEffect
{
    public int damageReduction = 3; // Amount of damage reduced per hit
    public int durationTurns = 3;   // How many turns it lasts

    public override void Apply(GameObject target)
    {
        Enemy enemy = null;
        if (target)
        {
            enemy = target.GetComponent<Enemy>();
        }
        
        if (enemy != null)
        {
            enemy.ApplyStatus(new StatusEffect(StatusType.Soak, damageReduction, durationTurns));
        }
    }
}
