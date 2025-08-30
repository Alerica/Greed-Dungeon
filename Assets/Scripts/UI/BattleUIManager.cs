using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject battleUIPanel;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private TMP_Text playerEnergyText;
    [SerializeField] private TMP_Text playerMaxEnergyText;
    [SerializeField] private TMP_Text currentTurnText;
    [SerializeField] private TMP_Text enemiesCountText;

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


}
