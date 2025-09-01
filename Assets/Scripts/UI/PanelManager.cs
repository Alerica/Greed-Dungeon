using System.Collections;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public CanvasGroup mainPanel;
    public CanvasGroup battlePanel;
    public CanvasGroup collectionPanel;

    private CanvasGroup currentPanel;

    private bool isSwitching = false;

    void Start()
    {
        currentPanel = mainPanel;
        mainPanel.gameObject.SetActive(true);
        battlePanel.gameObject.SetActive(false);
        collectionPanel.gameObject.SetActive(false);
    }

    public void OpenBattle() => StartCoroutine(SwitchPanel(battlePanel));
    public void OpenCollection() => StartCoroutine(SwitchPanel(collectionPanel));
    public void BackToMain() => StartCoroutine(SwitchPanel(mainPanel));

    private IEnumerator SwitchPanel(CanvasGroup target)
    {
        if(isSwitching) yield break;
        isSwitching = true;
        if (currentPanel == target) yield break;

        // Enable target
        target.gameObject.SetActive(true);
        target.alpha = 0f;
        target.transform.localPosition = new Vector3(Screen.width, 0, 0); // Start off to right

        // Animate fade out & slide left for current
        float t = 0f;
        Vector3 startPos = currentPanel.transform.localPosition;
        Vector3 endPos = new Vector3(-Screen.width, 0, 0);

        while (t < 1f)
        {
            t += Time.deltaTime * 2f; // speed
            currentPanel.alpha = Mathf.Lerp(1f, 0f, t);
            currentPanel.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            target.alpha = Mathf.Lerp(0f, 1f, t);
            target.transform.localPosition = Vector3.Lerp(new Vector3(Screen.width, 0, 0), Vector3.zero, t);
            yield return null;
        }

        // Disable old
        currentPanel.gameObject.SetActive(false);
        currentPanel = target;
        isSwitching = false;
    }
}
