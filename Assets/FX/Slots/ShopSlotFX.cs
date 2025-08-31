using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlotFX : MonoBehaviour
{
    [SerializeField] private CardData[] possibleCardsGold; // array of possible cards to win
    [SerializeField] private CardData[] possibleCardsSilver; // array of possible cards to win
    [SerializeField] private CardData[] possibleCardsBronze; // array of possible cards to win
    [SerializeField][Range(0f, 1f)] private float ChanceToWinGold = 0.05f; // slider in Inspector
    [SerializeField][Range(0f, 1f)] private float ChanceToWinSilver = 0.35f; // slider in Inspector
    [SerializeField][Range(0f, 1f)] private float ChanceToWinBronze = 0.6f; // slider in Inspector
    [SerializeField] private List<GameObject> texts;
    [SerializeField] private Image img;

    public void ShowRewardInSlot(CardData rewardCard, float fadeDuration = 0.5f)
    {
        if (img == null) return;

        // Set artwork and start transparent
        Debug.Log("Show reward: " + rewardCard.cardName);
        Debug.Log("Artwork: " + rewardCard.artwork);
        img.sprite = rewardCard.artwork;
        img.color = new Color(1, 1, 1, 0);
        img.gameObject.SetActive(true);

        // Fade in
        StartCoroutine(PopIn(img, fadeDuration));
    }

    private IEnumerator PopIn(Image img, float duration = 0.5f)
    {
        float t = 0f;

        // Start small and transparent
        img.transform.localScale = Vector3.zero;
        img.color = new Color(1, 1, 1, 0);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // Smooth scale up (ease out)
            float scale = Mathf.SmoothStep(0f, 1.5f, t);
            img.transform.localScale = new Vector3(scale, scale, scale);

            // Fade in
            img.color = new Color(1, 1, 1, Mathf.Lerp(0f, 1f, t));

            yield return null;
        }

        // Ensure final values
        img.transform.localScale = Vector3.one;
        img.color = Color.white;
    }


    public void GenerateNumber(GameObject slot)
    {
        // Ensure all text objects are active
        foreach (GameObject go in texts)
            go.SetActive(true);

        float roll = Random.value; // 0-1 random
        CardData rewardCard = null;

        if (roll < ChanceToWinGold)
        {
            // Gold: all 3 numbers same
            int winningNumber = Random.Range(1, 10);
            for (int i = 0; i < 3; i++)
                texts[i].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();

            if (possibleCardsGold.Length > 0)
                rewardCard = possibleCardsGold[Random.Range(0, possibleCardsGold.Length)];
        }
        else if (roll < ChanceToWinGold + ChanceToWinSilver)
        {
            // Silver: 2 numbers same
            int repeatedNumber = Random.Range(1, 10);
            int differentNumber = repeatedNumber;
            while (differentNumber == repeatedNumber)
                differentNumber = Random.Range(1, 10);

            int diffIndex = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
                texts[i].GetComponent<TextMeshProUGUI>().text = (i == diffIndex ? differentNumber : repeatedNumber).ToString();

            if (possibleCardsSilver.Length > 0)
                rewardCard = possibleCardsSilver[Random.Range(0, possibleCardsSilver.Length)];
        }
        else
        {
            // Bronze: all different numbers
            int num1 = Random.Range(1, 10);
            int num2 = num1;
            int num3 = num1;
            while (num2 == num1) num2 = Random.Range(1, 10);
            while (num3 == num1 || num3 == num2) num3 = Random.Range(1, 10);

            texts[0].GetComponent<TextMeshProUGUI>().text = num1.ToString();
            texts[1].GetComponent<TextMeshProUGUI>().text = num2.ToString();
            texts[2].GetComponent<TextMeshProUGUI>().text = num3.ToString();

            if (possibleCardsBronze.Length > 0)
                rewardCard = possibleCardsBronze[Random.Range(0, possibleCardsBronze.Length)];
        }

        // Give reward
        if (rewardCard != null)
        {
            CardData temp = rewardCard;
            // Add to deck
            BattleManager.Instance.AddCardToDeck(rewardCard);

            // Show reward in the slot
            ShowRewardInSlot(temp);
        }
    }
}
