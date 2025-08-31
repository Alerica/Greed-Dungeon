using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject battleUIPanel;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerEnergyText;
    [SerializeField] private TMP_Text playerMaxEnergyText;
    [SerializeField] private TMP_Text currentTurnText;
    [SerializeField] private TMP_Text currentStageText;
    [SerializeField] private TMP_Text enemiesCountText;
    [SerializeField] private TMP_Text cardCountText;

    [SerializeField] private FloatingText floatingTextPrefab;
    [SerializeField] private Canvas uiCanvas;

    public void ShowBattleUI()
    {
        battleUIPanel.SetActive(true);
    }

    public void HideBattleUI()
    {
        battleUIPanel.SetActive(false);
    }

    public void UpdatePlayerHealth(int health)
    {
        playerHealthText.text = $"{health}";
    }

    public void UpdatePlayerEnergy(int energy)
    {
        playerEnergyText.text = $"{energy}";
    }

    public void PlayEffectText(string message, Color color, Vector3 position)
    {
        SpawnFloatingText(message, color, position);
    }

    public void UpdateCurrentTurn(int turn)
    {
        currentTurnText.text = $"{turn}";
    }

    public void UpdatePlayerMaxEnergy(int maxEnergy)
    {
        playerMaxEnergyText.text = $"{maxEnergy}";
    }

    public void UpdateEnemiesCount()
    {
        enemiesCountText.text = $"{BattleManager.Instance.enemies.Count}";
    }

    public void UpdateCurrentStage(int stage)
    {
        currentStageText.text = $"{stage}";
    }

    public void UpdateCardCount(int count)
    {
        cardCountText.text = $"{count}";
    }

    private void SpawnFloatingText(string message, Color color, Vector3 worldPos)
    {
        FloatingText ft = Instantiate(floatingTextPrefab, uiCanvas.transform);
        ft.Play(message, color, worldPos);
    }



}
