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
    public BGAnimator bGAnimator;
    public BattleUIManager battleUIManager;
    public HorizontalCardHolder cardHolder;
    public TextVisualEffect turnBanner;

    public Transform energyTransform;
    public Transform hpTransform;

    [Header("Input Blocker")]
    [SerializeField] private GameObject inputBlocker;

    [Header("Choice Panel")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject rewardPanel;

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
        IncreaseStage();
        currentTurn = 0;
        Debug.Log($"Advancing to Stage {currentStage}");

        if (currentStage >= 2)
        {
            // Pause and wait for player decision
            ShowRetreatOrContinueUI();
        }
        else
        {
            // Continue automatically
            ContinueToStage();
        }
    }

    private void ShowRetreatOrContinueUI()
    {
        choicePanel.SetActive(true);
        Debug.Log("Choose: Retreat or Continue?");
    }

    // Called by UI Button
    public void OnContinue()
    {
        choicePanel.SetActive(false);
        Debug.Log("Player chose to continue!");
        ContinueToStage();
    }

    // Called by UI Button
    public void OnRetreat()
    {
        choicePanel.SetActive(false);
        Debug.Log("Player chose to retreat!");
        rewardPanel.SetActive(true);
    }

    private void ContinueToStage()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemyObj = enemySpawner.SpawnEnemy();
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                RegisterEnemy(enemyObj);
            }
        }

        StartCoroutine(Wait(2f, () => PlayerTurn()));
    }

    // Turn Management
    public void PlayerTurn()
    {
        inputBlocker.SetActive(false);
        for (int i = 0; i < cardDrawPerTurn; i++) cardHolder.DrawCard();
        currentState = BattleState.PlayerTurn;
        AddEnergy(1);
        IncreaseTurn();
        UpdatePlayerHP();

        Debug.Log($"Player's Turn {currentTurn}");
        if (turnBanner) turnBanner.ShowBanner("PLAYER TURN!", Color.cyan);
    }

    public void EndPlayerTurn()
    {
        inputBlocker.SetActive(true); 
        if (currentState != BattleState.PlayerTurn)
        {
            Debug.LogWarning("It's not the player's turn! or the game hasn’t started yet.");
            return;
        }

        StartCoroutine(EndPlayerTurnRoutine());
    }

    private IEnumerator EndPlayerTurnRoutine()
    {
        Debug.Log("Ending Player Turn");
        // small delay before next step
        yield return new WaitForSeconds(0.5f);

        if (AreAllEnemiesDead())
        {
            bGAnimator.StartAnimation();
            if (turnBanner) turnBanner.ShowBanner("ENEMY DEFEATED!", Color.yellow);
            yield return new WaitForSeconds(2f);
            NextStage();
        }
        else
        {
            Debug.Log("Player ends turn.");
            EnemyTurn();
        }
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

            Debug.Log($"Enemy attack with {enemyScript.GetAttackPower()}"); playerScript.TakeDamage(enemyScript.GetAttackPower());
            UpdatePlayerHP();
        }

        yield return new WaitForSeconds(0.5f); // small buffer
        PlayerTurn();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy.gameObject))
        {
            enemies.Remove(enemy.gameObject);
            AddEnergy(3);
        }
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
        battleUIManager.PlayEffectText($"+{energy}", Color.blue, energyTransform.position);
    }

    public void RemoveEnergy(int energy)
    {
        currentEnergy -= energy;
        battleUIManager.UpdatePlayerEnergy(currentEnergy);
        battleUIManager.PlayEffectText($"-{energy}", Color.blue, energyTransform.position);
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

    public void IncreaseStage()
    {
        currentStage++;
        battleUIManager.UpdateCurrentStage(currentStage);
    }

    public void UpdatePlayerHP()
    {
        battleUIManager.UpdatePlayerHealth(playerScript.CheckHealthInt());
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
