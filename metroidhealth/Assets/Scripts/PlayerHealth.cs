using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Color _originalColor;
    private Vector2 _respawnPosition;
    private PlayerMovement _playerMovement;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = GetComponent<SpriteRenderer>().color;
        _respawnPosition = transform.position;
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private IEnumerator DeathAndRespawn()
    {
        _playerMovement.IsDead = true;
        // reset game states
        
        _rigidbody2D.velocity = Vector2.zero;
        
        // death animation


        for (int i = 0; i < 5; i++)
        {
            _spriteRenderer.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
            yield return new WaitForSeconds(0.125f);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(0.125f);
        }

        transform.position = _respawnPosition;
        _playerMovement.IsDead = false;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        // check collision with spike/obstacle
        if (col.gameObject.CompareTag("Obstacle") && !_playerMovement.IsDead)
        {   
            StartCoroutine(DeathAndRespawn());
        }
    }
}
