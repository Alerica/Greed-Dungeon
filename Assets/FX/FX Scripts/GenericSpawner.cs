using UnityEngine;

public class GenericSpawner : MonoBehaviour
{
    
    public void Cast(GameObject spell)
    {
        // Instantiate tidal wave prefab and set its position and rotation
        GameObject Spellprefab = Instantiate(spell);
        Destroy(Spellprefab, 5f); // Destroy after 5 seconds to prevent clutter
    }
}
