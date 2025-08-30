using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class SlotFX : MonoBehaviour
{
    [SerializeField] private List<GameObject> texts;

    [SerializeField][Range(0f, 1f)] private float ChanceToWin = 0.3f; // slider in Inspector

    public void GenerateNumber()
    {
        // Step 1: Make sure all text objects are enabled
        foreach (GameObject go in texts)
        {
            go.SetActive(true);
        }

        float roll = Random.Range(0f, 1f);

        if (roll < ChanceToWin)
        {
            // Winning case all 3 numbers same
            int winningNumber = Random.Range(1, 10);
            texts[0].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
            texts[1].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
            texts[2].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();
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
