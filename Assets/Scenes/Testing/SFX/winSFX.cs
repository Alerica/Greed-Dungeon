using UnityEngine;
using GameAudio; 

public class winSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("win");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
