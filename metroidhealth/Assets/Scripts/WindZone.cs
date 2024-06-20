using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    // Constantly applies a force to the player
    
    [SerializeField] private Vector2 windForce;

    private Collider2D _collider2D;
    private Collider2D _playerCollider2D;
    private Rigidbody2D _playerRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        _playerCollider2D = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
        _playerRigidbody2D = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_collider2D.IsTouching(_playerCollider2D))
        {
            _playerRigidbody2D.AddForce(windForce, ForceMode2D.Force);
        }
    }
}
