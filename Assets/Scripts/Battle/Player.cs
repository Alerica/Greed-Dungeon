using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private string playerName = "Player";
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private float baseDefense = 5;
    [SerializeField] private float luck = 0.001f;
    [SerializeField] private bool isPlayerAlive = false;
    [SerializeField] private Image HitFX;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public void Heal(float amount)
    {
        Debug.Log($"Heal player by {amount}");
        BattleManager.Instance.battleUIManager.PlayEffectText($"+{amount}", Color.green, BattleManager.Instance.hpTransform.position);
        if (health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }
    public void TakeDamage(float rawDamage)
    {
        Debug.Log($"Player take {rawDamage}");
        BattleManager.Instance.battleUIManager.PlayEffectText($"-{rawDamage}", Color.red, BattleManager.Instance.hpTransform.position);
        float actualDefense = baseDefense;
        // Attack 100 
        // defense 10  = 8% dr
        // defense 66 = 33 dr%
        // defense 200 = 60 dr%
        // defense 900 = 90 dr%
        float damageReduction = 100 / (100 + actualDefense);
        float actualDamage = rawDamage * damageReduction;
        health -= actualDamage;
        FadeOut();
        if (health < 0)
        {
            health = 0;
            Die();
        }
    }

    public float CheckHealth()
    {
        return health;
    }

    public int CheckHealthInt()
    {
        return Mathf.RoundToInt(health);
    }

    public void Die()
    {

    }

    public IEnumerator ProcessTurnEffectsCoroutine()
    {
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = statusEffects[i];

            switch (effect.type)
            {
                case StatusType.Burn:
                    break;
                case StatusType.Bleed:
                    break;
                case StatusType.Stun:
                    break;
                case StatusType.Frost:
                    break;
            }

            effect.turnsRemaining--;
            if (effect.turnsRemaining <= 0)
                statusEffects.RemoveAt(i);
        }
        yield return new WaitForSeconds(0.1f);
    }
    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine());
    }

    private System.Collections.IEnumerator FadeCoroutine()
    {
        Color startColor = HitFX.color;
        Color endColor = startColor;
        startColor.a = 0.5f;  // start with alpha = 0.5
        endColor.a = 0f;      // end with alpha = 0

        HitFX.color = startColor;

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime;
            float normalized = t / 1;
            HitFX.color = Color.Lerp(startColor, endColor, normalized);
            yield return null;
        }

        // Ensure it ends at alpha 0
        HitFX.color = endColor;
    }
}
