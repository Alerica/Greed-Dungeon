using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string playerName = "Player";
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private float baseDefense = 5;
    [SerializeField] private float luck = 0.001f;
    [SerializeField] private bool isPlayerAlive = false;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    public void Heal(float amount)
    {
        Debug.Log($"Heal player by {amount}");
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
        float actualDefense = baseDefense;
        float damageReduction = 100 / 100 + actualDefense;
        float actualDamage = rawDamage * damageReduction;
        health -= actualDamage;
        if (health < 0)
        {
            Die();
        }
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

}
