using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Info for teleporter's destination
    
    [SerializeField] private Transform destination;

    public Transform GetDestination()
    {
        return destination;
    }
}