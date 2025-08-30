using System.Linq;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    [SerializeField]private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private void Awake()
    {
        Destroy(gameObject, 2f);
    }

    public Transform getStartPoint()
    {
        return startPoint;
    }
    public Transform getEndPoint()
    {
        return endPoint;
    }
}
