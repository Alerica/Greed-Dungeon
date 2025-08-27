using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string enemyName = "Goblin";
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float defense = 5;
    [SerializeField] private float health = 100;
    [SerializeField] private float attackPower = 10;


    void Start()
    {

    }

    void Update()
    {

    }

    public void TakeDamage(float amount)
    {
        // Damage calculation : raw damage * 100 / (100 + defense)
        health -= amount * (100 / 100 + defense);
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public float GetHealth()
    {
        return health;
    }
    
    public void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        Destroy(gameObject);
    }
}
