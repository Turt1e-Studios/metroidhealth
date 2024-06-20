using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantForce : MonoBehaviour
{
    // This script applies an instant force to the player for the explosion orb
    
    [SerializeField] private float force;
    [SerializeField] private float minMultiplier = 0.5f;
    [SerializeField] private float duration;
    
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

    private void Update()
    {
        if (_collider2D.IsTouching(_playerCollider2D) && Input.GetKeyDown(KeyCode.Space))
        {
            _playerCollider2D.gameObject.GetComponent<PlayerMovement>().SetHorizontalMovement(false);
            StartCoroutine(ResetControls());
            float ratioX = Math.Abs((_playerCollider2D.transform.position - transform.position).x) / (_collider2D.bounds.size.x / 2);
            float ratioY = Math.Abs((_playerCollider2D.transform.position - transform.position).y) / (_collider2D.bounds.size.y / 2);
            Vector2 multiplier = new Vector2(Mathf.Lerp(1, minMultiplier, ratioX), Mathf.Lerp(1, minMultiplier, ratioY));
            print(multiplier.x);
            Vector2 direction = new Vector2((_playerCollider2D.transform.position - transform.position).x, (_playerCollider2D.transform.position - transform.position).y).normalized;
            _playerRigidbody2D.AddForce(force * (multiplier * direction), ForceMode2D.Impulse);
        }
    }

    private IEnumerator ResetControls()
    {
        yield return new WaitForSeconds(duration);
        print("player can now move again");
        _playerCollider2D.gameObject.GetComponent<PlayerMovement>().SetHorizontalMovement(true);
    }

    // private void OnTriggerStay2D(Collider2D col)
    // {
    //     if (col.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
    //     {
    //         col.GetComponent<Rigidbody2D>().AddForce(force * (new Vector2(1 / (col.transform.position - transform.position).x, 1 / (col.transform.position - transform.position).y)), ForceMode2D.Impulse);
    //     }
    // }
}
