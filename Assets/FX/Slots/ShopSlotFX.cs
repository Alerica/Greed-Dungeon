using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotFX : MonoBehaviour
{
    [SerializeField] private CardData[] possibleCardsGold;
    [SerializeField] private CardData[] possibleCardsSilver;
    [SerializeField] private CardData[] possibleCardsBronze;

    [SerializeField][Range(0f, 1f)] private float ChanceToWinGold = 0.05f;
    [SerializeField][Range(0f, 1f)] private float ChanceToWinSilver = 0.35f;
    [SerializeField][Range(0f, 1f)] private float ChanceToWinBronze = 0.6f;

    [SerializeField] private List<GameObject> texts;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool rewardShown = false;

    public void ShowRewardInSlot(CardData rewardCard, float fadeDuration = 0.5f)
    {
        if (img == null) return;

        Debug.Log("Show reward: " + rewardCard.cardName);

        // Assign visuals
        img.sprite = rewardCard.artwork;
        Title.text = rewardCard.cardName;
        Description.text = rewardCard.description;

        // Reset for animation
        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);

        rewardShown = true;

        // Fade in with scale
        StartCoroutine(PopIn(fadeDuration));
    }

    private IEnumerator PopIn(float duration = 0.5f)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            // Smooth scale up (ease out)
            float scale = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = new Vector3(scale, scale, scale);

            // Fade alpha
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Final values
        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }

    private void Update()
    {
        // Wait for a click if reward is shown
        if (rewardShown && Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject); // remove slot after click
        }
    }

    public void GenerateNumber(GameObject slot)
    {
        foreach (GameObject go in texts)
            go.SetActive(true);

        float roll = Random.value;
        CardData rewardCard = null;

        if (roll < ChanceToWinGold)
        {
            int winningNumber = Random.Range(1, 10);
            for (int i = 0; i < 3; i++)
                texts[i].GetComponent<TextMeshProUGUI>().text = winningNumber.ToString();

            if (possibleCardsGold.Length > 0)
                rewardCard = possibleCardsGold[Random.Range(0, possibleCardsGold.Length)];
        }
        else if (roll < ChanceToWinGold + ChanceToWinSilver)
        {
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

        if (rewardCard != null)
        {
            BattleManager.Instance.AddCardToDeck(rewardCard);
            ShowRewardInSlot(rewardCard);
        }
    }
}
