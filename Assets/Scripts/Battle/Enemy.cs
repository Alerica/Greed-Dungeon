using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName = "Goblin";
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private float defense = 5;
    [SerializeField] private float attackPower = 10;
    [SerializeField] private float enemyDamageModifier = 1;

    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [Header("Actual Stats (Read-Only)")]
    public bool isDamageReductionApplied = false;
    public bool isBoss = false;
    public bool isStunned = false;

    [Header("SFX")]
    public TextVisualEffect turnBanner;

    public void TakeDamage(float amount)
    {
        StartCoroutine(TakeDamageCoroutine(amount));
    }

    public IEnumerator TakeDamageCoroutine(float amount, string source = null)
    {
        if(isDamageReductionApplied)
            amount *= 0.75f; // Example: 25% damage reduction
        health -= amount * (100 / (100 + defense));
        // Show floating text if banner exists
        if (turnBanner != null)
        {
            string message = source != null ? $"{source}! -{Mathf.RoundToInt(amount)}!" : $"-{Mathf.RoundToInt(amount)}";
            Color color = source switch
            {
                "Burn" => Color.red,
                "Bleed" => new Color(0.6f, 0, 0), // dark red
                _ => Color.white
            };

            yield return turnBanner.ShowBannerCoroutine(message, color, 0.6f); // wait for banner animation
        }

        if (health <= 0)
            Die();
    }

    public void ApplyStatus(StatusEffect effect)
    {
        statusEffects.Add(effect);
        Debug.Log($"{enemyName} received {effect.type} for {effect.turnsRemaining} turns");

        // Assign once if needed
        if (turnBanner == null)
        {
            GameObject bannerObj = GameObject.FindGameObjectWithTag("TurnBanner");
            if (bannerObj != null)
                turnBanner = bannerObj.GetComponent<TextVisualEffect>();
        }

        // Always show banner, even if already assigned
        if (turnBanner != null)
        {
            string bannerMessage = effect.type.ToString(); // e.g., "Stun", "Frost"
            Color bannerColor = GetEffectColor(effect.type);
            turnBanner.ShowBanner(bannerMessage, bannerColor);
        }
    }


    public IEnumerator ApplyStatusCoroutine(StatusEffect effect)
    {
        statusEffects.Add(effect);
        Debug.Log($"{enemyName} received {effect.type} for {effect.turnsRemaining} turns");

        if (turnBanner == null)
        {
            GameObject bannerObj = GameObject.FindGameObjectWithTag("TurnBanner");
            if (bannerObj != null)
                turnBanner = bannerObj.GetComponent<TextVisualEffect>(); // assuming TurnBanner is your banner class
        }

        if (turnBanner != null)
        {
            string bannerMessage = effect.type.ToString(); // e.g., "Stun", "Frost"
            Color bannerColor = GetEffectColor(effect.type);
            yield return turnBanner.ShowBannerCoroutine(bannerMessage, bannerColor); // wait for animation
        }
    }


    private Color GetEffectColor(StatusType type)
    {
        switch (type)
        {
            case StatusType.Burn: return Color.red;
            case StatusType.Stun: return Color.yellow;
            case StatusType.Frost: return Color.cyan;
            case StatusType.Bleed: return Color.red;
            case StatusType.DefenseReduction: return Color.magenta;
            case StatusType.Leech: return Color.green;
            case StatusType.Soak: return Color.blue;
            case StatusType.Gnawed: return new Color(0.8f, 0.4f, 0f); // orange
            default: return Color.white;
        }
    }

    // Call this at the **start or end of each enemy turn**
    public IEnumerator ProcessTurnEffectsCoroutine()
    {
        isStunned = false; 
        enemyDamageModifier = 1f; 
        for (int i = statusEffects.Count - 1; i >= 0; i--)
        {
            var effect = statusEffects[i];

            switch (effect.type)
            {
                case StatusType.Burn:
                    yield return TakeDamageCoroutine(effect.value, "Burn");
                    break;
                case StatusType.Bleed:
                    yield return TakeDamageCoroutine(effect.value, "Bleed");
                    break;
                case StatusType.Stun:
                    isStunned = true;
                    yield return turnBanner.ShowBannerCoroutine("Stunned!", Color.yellow);
                    break;
                case StatusType.Frost:
                    enemyDamageModifier = 5f;
                    break;
                case StatusType.DefenseReduction:
                    break;
                case StatusType.Leech:
                    break;
                case StatusType.Soak:
                    break;
                case StatusType.Gnawed:
                    break;
                    // Add other effects here
            }

            effect.turnsRemaining--;
            if (effect.turnsRemaining <= 0)
                statusEffects.RemoveAt(i);
        }
    }

    public float GetHealth() => health;

    public float SetHealth(float newHealth)
    {
        maxHealth = newHealth;
        health = newHealth;
        return health;
    }

    public float GetAttackPower()
    {
        return attackPower;
    }

    public void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        if(isBoss)
        {
            BattleManager.Instance.bossDefeated++;
        }
        BattleManager.Instance.RemoveEnemy(this);
        BattleManager.Instance.EndPlayerTurn();
        
    }
}
