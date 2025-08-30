using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Draw Cards")]
public class DrawCardEffect : CardEffect
{
    public int drawAmount = 2;

    public override void Apply(GameObject target)
    {
        for (int i = 0; i < drawAmount; i++)
        {
            BattleManager.Instance.cardHolder.DrawCard();
        }
        Debug.Log($"Drew {drawAmount} card(s)");
    }
}
