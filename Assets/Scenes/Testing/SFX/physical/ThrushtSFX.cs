using UnityEngine;
using GameAudio;

public class ThrushtSFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      AudioManager.I?.PlaySFX("thrusht");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
