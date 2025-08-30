using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = text.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = text.gameObject.AddComponent<CanvasGroup>();
    }

    public void Play(string message, Color color, Vector3 worldPos)
    {
        text.text = message;
        text.color = color;

        transform.position = worldPos;
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;

        // Scale in
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 0.2f);

        // Move upward while fading
        transform.DOMoveY(transform.position.y + 50f, 1f).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(0f, 1f).SetDelay(0.5f);

        // Destroy after animation
        Destroy(gameObject, 1.5f);
    }
}
