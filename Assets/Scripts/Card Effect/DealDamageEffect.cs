using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage")]
public class DealDamageEffect : CardEffect
{
    public int damageAmount = 5;

    public override void Apply(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount);
            Debug.Log("Dealt " + damageAmount + " damage to " + enemy.name);
        }
    }
}

