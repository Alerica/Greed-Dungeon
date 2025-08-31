using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class VideoFadeOut : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // The VideoPlayer component
    public RawImage rawImage;            // The RawImage displaying the video
    public float fadeDuration = 2f;      // Duration of fade out in seconds
    public float delayBeforeFade = 0f;   // Optional delay before starting fade

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (delayBeforeFade > 0f)
            yield return new WaitForSeconds(delayBeforeFade);

        float t = 0f;
        Color originalColor = rawImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        rawImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}