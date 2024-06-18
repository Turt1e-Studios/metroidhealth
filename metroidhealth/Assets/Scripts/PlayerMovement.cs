using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private Vector2 _velocity;
    private Rigidbody2D _rigidbody2D;
    private bool _isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),0);
        _velocity = input.normalized;
        transform.Translate(_velocity * (Time.deltaTime * speed));
        
        // Check for jumps on the ground or in the air
        if (Input.GetKeyDown(KeyCode.W) && _isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }
    
    // Reset variables after hitting the ground again
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Object")))
        {
            _isGrounded = true;
        }
    }
 
    // Player leaves the ground
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(("Object")))
        {
            _isGrounded = false;
        }
    }
    
    bool IsGrounded()
    {
        return GetComponent<Rigidbody>().velocity.y == 0;
    }
}
