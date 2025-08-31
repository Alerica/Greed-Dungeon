using UnityEngine;
using GameAudio;

public class ArrowSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("arrow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
