using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpingPower = 50f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public bool canDoubleJump;

    private float _horizontal;
    private bool _isFacingRight = true;
    private bool _hasJumped;

    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            _hasJumped = false;
        }

        if (Input.GetButtonDown("Jump") && (IsGrounded() || (canDoubleJump && !_hasJumped)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            _hasJumped = true;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}