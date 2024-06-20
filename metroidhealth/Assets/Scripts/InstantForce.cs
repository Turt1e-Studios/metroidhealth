using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantForce : MonoBehaviour
{
    [SerializeField] private float force;
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
            _playerRigidbody2D.AddForce(force * (new Vector2(1 / (_playerCollider2D.transform.position - transform.position).x, 1 / (_playerCollider2D.transform.position - transform.position).y)), ForceMode2D.Impulse);
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
