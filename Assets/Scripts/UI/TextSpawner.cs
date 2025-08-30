using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    public FloatingText floatingTextPrefab;
    public Canvas uiCanvas;

    public void SpawnText(string message, Color color, Vector3 worldPos)
    {
        FloatingText ft = Instantiate(floatingTextPrefab, uiCanvas.transform);
        ft.Play(message, color, worldPos);
    }
}

