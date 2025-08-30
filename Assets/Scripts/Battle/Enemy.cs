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

    private List<EnemyStatusEffect> statusEffects = new List<EnemyStatusEffect>();

    [Header("SFX")]
    public TextVisualEffect turnBanner;

    public void TakeDamage(float amount)
    {
        health -= amount * (100 / (100 + defense));
        
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount, string source = null)
    {
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

            turnBanner.ShowBanner(message, color);
        }

        if (health <= 0)
            Die();
    }

    public IEnumerator TakeDamageCoroutine(float amount, string source = null)
    {
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

    public void ApplyStatus(EnemyStatusEffect effect)
    {
        statusEffects.Add(effect);
        Debug.Log($"{enemyName} received {effect.type} for {effect.turnsRemaining} turns");

        if (turnBanner == null)
        {
            GameObject bannerObj = GameObject.FindGameObjectWithTag("TurnBanner");
            if (bannerObj != null)
                turnBanner = bannerObj.GetComponent<TextVisualEffect>();
            string bannerMessage = effect.type.ToString(); // e.g., "Stun", "Frost"
            Color bannerColor = GetEffectColor(effect.type);
            turnBanner.ShowBanner(bannerMessage, bannerColor);
        }
    }
    
    public IEnumerator ApplyStatusCoroutine(EnemyStatusEffect effect)
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
                    if (turnBanner != null)
                        yield return turnBanner.ShowBannerCoroutine("Stunned!", Color.yellow);
                    break;
                case StatusType.Frost:
                    enemyDamageModifier = Mathf.Max(0, enemyDamageModifier - effect.value);
                    break;
                // Add other effects here
            }

            effect.turnsRemaining--;
            if (effect.turnsRemaining <= 0)
                statusEffects.RemoveAt(i);
        }
    }

    public float GetHealth() => health;

    public void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        Destroy(gameObject);
    }
}
