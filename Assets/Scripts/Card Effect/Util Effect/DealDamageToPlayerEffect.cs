using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Deal Damage To Player")]
public class DealDamageToPlayerEffect : CardEffect
{
    public int damageAmount = 5;

    public override void Apply(GameObject target)
    {
        BattleManager.Instance.playerScript.TakeDamage(damageAmount);
        BattleManager.Instance.battleUIManager.UpdatePlayerHealth(BattleManager.Instance.playerScript.CheckHealthInt());
        Debug.Log($"Player took {damageAmount} damage");
    }
}
