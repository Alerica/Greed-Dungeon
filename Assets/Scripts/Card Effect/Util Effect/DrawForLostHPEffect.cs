using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Draw For Lost HP")]
public class DrawForLostHPEffect : CardEffect
{
    [Tooltip("Draw 1 card for each X HP lost")]
    public int hpPerCard = 10;
    public override void Apply(GameObject target)
    {
        int currentHP = Mathf.RoundToInt(BattleManager.Instance.playerScript.CheckHealth());
        int maxHP = 100;

        int lostHP = maxHP - currentHP;
        int cardsToDraw = lostHP / hpPerCard;

        for (int i = 0; i < cardsToDraw; i++)
        {
            BattleManager.Instance.cardHolder.DrawCard();
        }

        Debug.Log($"Lost {lostHP} HP → Drew {cardsToDraw} card(s)");
    }
}
