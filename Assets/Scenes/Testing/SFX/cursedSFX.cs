using UnityEngine;
using GameAudio; 

public class cursed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("cursed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
