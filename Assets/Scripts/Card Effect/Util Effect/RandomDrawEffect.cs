using DiceSystem2D;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Random Draw")]
public class RandomDrawEffect : CardEffect
{
    public int extraDrawOnSix = 2; 
    public int normalDraw = 1;
    public override void Apply(GameObject target)
    {
        int rolledSum = 0;

        DiceManager2DUI diceManager2DUI = FindFirstObjectByType<DiceManager2DUI>();
        diceManager2DUI.AddAndRoll(
            count: 1,
            sum => {
            Debug.Log($"Dice rolled: {sum}");
            rolledSum = sum; // store the sum here
            },
            destroyAfter: true,
            destroyDelay: 0.25f // pausenya 
        );

        int cardsToDraw;
        if(rolledSum == 6)
        {
            cardsToDraw = extraDrawOnSix;
        }
        else
        {
            cardsToDraw = normalDraw;
        }
 
        for (int i = 0; i < cardsToDraw; i++)
        {
            BattleManager.Instance.cardHolder.DrawCard();
        }

        Debug.Log($"Rolled {rolledSum} → Drew {cardsToDraw} card(s)");
    }
}
