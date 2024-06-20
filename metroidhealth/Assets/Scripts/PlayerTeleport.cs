using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    // Teleport the player when they interact with a teleporter
    
    private GameObject _currentTeleporter;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentTeleporter != null)
            {
                transform.position = _currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            _currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            if (collision.gameObject == _currentTeleporter)
            {
                _currentTeleporter = null;
            }
        }
    }
}