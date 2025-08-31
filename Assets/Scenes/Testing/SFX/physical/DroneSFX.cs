using UnityEngine;
using GameAudio;

public class DroneSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("drone");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
