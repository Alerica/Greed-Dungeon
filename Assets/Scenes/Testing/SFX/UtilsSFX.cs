using UnityEngine;
using GameAudio; 

public class UtilsSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("utils");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
