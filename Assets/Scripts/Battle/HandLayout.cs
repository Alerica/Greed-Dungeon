using UnityEngine;

public class HandLayout : MonoBehaviour
{
    public float spacing = 2f;

    public void RepositionCards()
    {
        int count = transform.childCount;
        float startX = -(count - 1) * spacing / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform card = transform.GetChild(i);
            Vector3 targetPos = new Vector3(startX + i * spacing, 0, 0);
            card.localPosition = targetPos;
        }
    }
}
