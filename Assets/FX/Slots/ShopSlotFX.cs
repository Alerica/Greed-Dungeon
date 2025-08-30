using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSlotFX : MonoBehaviour
{
    [SerializeField] private CardData[] possibleCardsGold; // array of possible cards to win
    [SerializeField] private CardData[] possibleCardsSilver; // array of possible cards to win
    [SerializeField] private CardData[] possibleCardsBronze; // array of possible cards to win
    [SerializeField][Range(0f, 1f)] private float ChanceToWinGold = 0.05f; // slider in Inspector
    [SerializeField][Range(0f, 1f)] private float ChanceToWinSilver = 0.35f; // slider in Inspector
    [SerializeField][Range(0f, 1f)] private float ChanceToWinBronze = 0.6f; // slider in Inspector
    [SerializeField] private List<GameObject> texts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GenerateNumber()
    {
        // Step 1: Make sure all text objects are enabled
        foreach (GameObject go in texts)
        {
            go.SetActive(true);
        }

        float roll = Random.Range(0f, 1f);

        if (roll < ChanceToWinGold)
        {
            // Winning case all 3 numbers same
            int winningNumber = Random.Range(1, 10);
            texts[0].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
            texts[1].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
            texts[2].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
            // Award a random gold card
            int cardIndex = Random.Range(0, possibleCardsGold.Length);

        }
        else
        {
            // Losing case ensure not all 3 are the same
            int number1 = Random.Range(1, 10);
            int number2 = Random.Range(1, 10);
            int number3 = Random.Range(1, 10);

            if (number1 == number2 && number2 == number3)
            {
                number3 = (number3 % 9) + 1; // guarantees a different number
            }

            texts[0].GetComponent<TextMeshProUGUI>().text = number1.ToString();
            texts[1].GetComponent<TextMeshProUGUI>().text = number2.ToString();
            texts[2].GetComponent<TextMeshProUGUI>().text = number3.ToString();
        }
    }
}
