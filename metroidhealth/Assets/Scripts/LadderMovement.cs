using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    // Movement of the player when on a ladder
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 8f;
    private float _vertical;
    private bool _isLadder;
    private bool _isClimbing;

    private bool _hasResetGravity;

    void Update()
    {
        _vertical = Input.GetAxisRaw("Vertical");

        if (_isLadder && Mathf.Abs(_vertical) > 0f)
        {
            _isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, _vertical * speed);
            _hasResetGravity = false;
        }
        else if (!_hasResetGravity)
        {
            rb.gravityScale = 9.8f;
            _hasResetGravity = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _isLadder = false;
            _isClimbing = false;
        }
    }
}