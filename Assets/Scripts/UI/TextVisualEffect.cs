using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
public class TextVisualEffect : MonoBehaviour
{
    public TextMeshProUGUI bannerText;
    public float showDuration = 2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = bannerText.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = bannerText.gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public IEnumerator ShowBannerCoroutine(string message, Color color)
    {
        bannerText.text = message;
        bannerText.color = color;

        bannerText.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        bannerText.transform.DOScale(1.2f, 0.5f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 0.3f);

        yield return new WaitForSeconds(showDuration);

        canvasGroup.DOFade(0f, 0.5f);
        bannerText.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(0.5f); // wait for fade out
    }

    public void ShowBanner(string message, Color color)
    {
        StartCoroutine(ShowBannerCoroutine(message, color));
    }
}
