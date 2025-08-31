using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Random Draw")]
public class RandomDrawEffect : CardEffect
{
    public int extraDrawOnSix = 2; 
    public int normalDraw = 1;
    public override void Apply(GameObject target)
    {
        int roll = Random.Range(1, 7); // 1 to 6 inclusive
        int cardsToDraw = (roll == 6) ? extraDrawOnSix : 1;

        for (int i = 0; i < cardsToDraw; i++)
        {
            BattleManager.Instance.cardHolder.DrawCard();
        }

        Debug.Log($"Rolled {roll} → Drew {cardsToDraw} card(s)");
    }
}
