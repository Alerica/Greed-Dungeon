using UnityEngine;
using GameAudio;
public class FireShoot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.I?.PlaySFX("CrossBow");
        AudioManager.I?.PlaySFX("FireE");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
