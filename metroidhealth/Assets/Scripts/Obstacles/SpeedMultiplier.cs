using UnityEngine;

public class SpeedMultiplier : MonoBehaviour
{
    // Multiply the player's speed
    
    [SerializeField] private float speedMultiplier = 2f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerMovement>().MultiplyVelocity(speedMultiplier);
        }
    }
}
