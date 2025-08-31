using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Heal Player")]
public class HealPlayer : CardEffect
{
    public int healAmount = 5;

    public override void Apply(GameObject target)
    {
        BattleManager.Instance.playerScript.Heal(healAmount);
        BattleManager.Instance.battleUIManager.UpdatePlayerHealth(BattleManager.Instance.playerScript.CheckHealthInt());
        Debug.Log($"Player healed for {healAmount}");
    }
}
