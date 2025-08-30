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
    }

    public void Roll()
    {
        if (activeRoll == null)
        {
            // Instantiate as child of this CollectionPanel
            activeRoll = Instantiate(rollPrefab, transform);

            StartCoroutine(LerpDown(activeRoll));
        }
    }

    private System.Collections.IEnumerator LerpDown(GameObject obj)
    {
        // Use prefab's local X/Z, but override Y with spawnY
        Vector3 startPos = new Vector3(obj.transform.localPosition.x, spawnY, obj.transform.localPosition.z);
        Vector3 endPos = new Vector3(obj.transform.localPosition.x, endY, obj.transform.localPosition.z);

        obj.transform.localPosition = startPos;

        float distance = Vector3.Distance(startPos, endPos);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rollSpeed / distance;
            obj.transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        obj.transform.localPosition = endPos;
    }
}
