using UnityEngine;
using GameAudio;

public class WindSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("wind");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
