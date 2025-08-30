using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    BattleState currentState = BattleState.NotStarted;

    [Header("Deck Settings")]
    public List<CardData> deckList = new List<CardData>();
    private Queue<CardData> deck = new Queue<CardData>();

    [Header("Enemy Settings")]
    public EnemySpawner enemySpawner;
    public int enemiesToSpawn = 1;
    public List<GameObject> enemies = new List<GameObject>();

    [Header("Player")]
    public GameObject player;
    public Player playerScript;

    [Header("Battle Settings")]
    public int initialCardsInHand = 5;
    public int cardDrawPerTurn = 1;
    private int currentEnergy = 5;
    private int maxEnergy = 10;

    public int currentStage = 0;
    public int currentTurn = 0;

    [Header("References")]
    public BattleUIManager battleUIManager;
    public HorizontalCardHolder cardHolder;
    public TextVisualEffect turnBanner;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ShuffleDeck(deckList);
        if (playerScript == null) player.GetComponent<Player>();
    }

    public void StartBattle()
    {
        if (currentState != BattleState.NotStarted)
        {
            Debug.Log("Battle`s already started!");
            return;
        }

        StartCoroutine(StartBattleRoutine());
    }

    private IEnumerator StartBattleRoutine()
    {
        Debug.Log("Battle Started!");
        if (turnBanner) turnBanner.ShowBanner("BATTLE START!", Color.yellow);
        yield return Wait(0.2f, () => DoNothing());

        for (int i = 0; i < initialCardsInHand; i++)
        {
            yield return Wait(0.2f, () =>
            {
                cardHolder.DrawCard();
            });
        }

        yield return Wait(1f, () => NextStage());
    }

    // Stage Management
    public void NextStage()
    {
        currentStage++;
        currentTurn = 0;
        Debug.Log($"Advancing to Stage {currentStage}");
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemyObj = enemySpawner.SpawnEnemy();
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                RegisterEnemy(enemyObj);
            }
        }
        StartCoroutine(Wait(1f, () => PlayerTurn()));
    }

    // Turn Management
    public void PlayerTurn()
    {
        for (int i = 0; i < cardDrawPerTurn; i++) cardHolder.DrawCard();
        currentState = BattleState.PlayerTurn;
        AddEnergy(1);
        IncreaseTurn();

        Debug.Log($"Player's Turn {currentTurn}");
        if (turnBanner) turnBanner.ShowBanner("PLAYER TURN!", Color.cyan);
    }

    public void EndPlayerTurn()
    {
        if (currentState != BattleState.PlayerTurn)
        {
            Debug.LogWarning("It's not the player's turn! or the game haven`t started yet.");
            return;
        }

        Debug.Log("Player ends turn.");
        EnemyTurn();
    }

    public void EnemyTurn()
    {
        currentState = BattleState.EnemyTurn;

        Debug.Log("Enemy's Turn");

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        if (turnBanner) yield return turnBanner.ShowBannerCoroutine("ENEMY TURN!", Color.red);

        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                yield return enemyScript.ProcessTurnEffectsCoroutine();
                Debug.Log($"{enemy.name} finished turn effects!");
            }

            playerScript.TakeDamage(enemyScript.GetAttackPower());
        }

        yield return new WaitForSeconds(0.5f); // small buffer
        PlayerTurn();
    }


    public void DoNothing()
    {
        Debug.Log("Nothing Function called");
    }

    public IEnumerator Wait(float seconds, System.Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
    // Enemy Management
    public void RegisterEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public bool AreAllEnemiesDead()
    {
        return enemies.Count == 0;
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    // Deck Management
    public CardData DrawFromDeck()
    {
        if (deck.Count == 0)
        {
            Debug.Log("Deck is empty!");
            return null;
        }
        Debug.Log("Drew card: " + deck.Peek().cardName);
        return deck.Dequeue();
    }

    public void ShuffleDeck(List<CardData> list)
    {
        System.Random rng = new System.Random();
        List<CardData> shuffled = new List<CardData>(list);
        int n = shuffled.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardData value = shuffled[k];
            shuffled[k] = shuffled[n];
            shuffled[n] = value;
        }

        deck = new Queue<CardData>(shuffled);
    }

    public void AddEnergy(int energy)
    {
        currentEnergy += energy;
        battleUIManager.UpdatePlayerEnergy(currentEnergy);
    }

    public void RemoveEnergy(int energy)
    {
        currentEnergy -= energy;
        battleUIManager.UpdatePlayerEnergy(currentEnergy);
    }

    public int GetCurrentEnergy()
    {
        return currentEnergy;
    }

    public void IncreaseTurn()
    {
        currentTurn++;
        battleUIManager.UpdateCurrentTurn(currentTurn);
    }
}

public enum BattleState
{
    NotStarted,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Defeat
}
