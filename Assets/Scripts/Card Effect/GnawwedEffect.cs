using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Gnawwed")]
public class GnawwedEffect : CardEffect
{
    public int GnwawwedDamage = 3;
    public int durationTurns = 3;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyStatus(new StatusEffect(StatusType.Gnawed, GnwawwedDamage, durationTurns));
        }
    }
}
