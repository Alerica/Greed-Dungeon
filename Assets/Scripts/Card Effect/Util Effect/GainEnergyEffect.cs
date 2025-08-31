using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Gain Energy")]
public class GainEnergyEffect : CardEffect
{
    public int energyAmount = 1;

    public override void Apply(GameObject target)
    {
        BattleManager.Instance.AddEnergy(energyAmount);
        Debug.Log($"Gained {energyAmount} energy");
    }
}

