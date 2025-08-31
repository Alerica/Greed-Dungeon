using UnityEngine;
using GameAudio;
public class IceSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       AudioManager.I?.PlaySFX("ice"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
