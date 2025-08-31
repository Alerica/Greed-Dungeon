using System;
using TMPro;
using UnityEngine;

[Serializable]
public class MultiLineText
{
    [TextArea(3, 10)]
    public string text;
}

public class CollectionPanel : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private MultiLineText[] texts;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private TextMeshProUGUI points;

    [Header("Roll Settings")]
    [SerializeField] private GameObject rollPrefab;
    [SerializeField] private float spawnY = 10f;   // start height
    [SerializeField] private float endY = -5f;     // target height
    [SerializeField] private float rollSpeed = 2f; // units per second

    private GameObject activeRoll;

    void Start()
    {
        string randomText = texts[UnityEngine.Random.Range(0, texts.Length)].text;
        textPrefab.GetComponent<TextMeshProUGUI>().text = randomText;
        UpdatePointUI();
    }

    public void Roll()
    {
        if (BattleManager.Instance.gamePoints >= 1500)
        {
            BattleManager.Instance.gamePoints -= 1500;
            UpdatePointUI();

            if (activeRoll == null)
            {
                // Instantiate as child of this CollectionPanel
                activeRoll = Instantiate(rollPrefab, transform);
            }
        }
        else
        {
            textPrefab.GetComponent<TextMeshProUGUI>().text = "Sorry honey but you will need 1500 biscuits to try your luck";
        }
        
    }

    public void UpdatePointUI()
    {
        points.text = BattleManager.Instance.gamePoints.ToString();
    }
}
