using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canDoubleJump;
    public bool canDash;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpingPower = 50f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    private float _horizontal;
    private bool _isFacingRight = true;
    private bool _hasJumped;
    
    private bool _canDash = true;
    private bool _isDashing;

    void Update()
    {
        if (_isDashing)
        {
            return;
        }
        
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            _hasJumped = false;
            _canDash = true;
        }

        if (Input.GetKeyDown(KeyCode.W) && (IsGrounded() || (canDoubleJump && !_hasJumped)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            _hasJumped = true;
        }

        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (_isDashing)
        {
            return;
        }
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
    
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        _isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        //_canDash = true;
    }
}