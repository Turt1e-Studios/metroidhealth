using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canDoubleJump;
    public bool canDash;
    public bool canWallJump;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float airSpeed = 10f;
    [SerializeField] private float jumpingPower = 50f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpCooldown = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private float _horizontal;
    private bool _isFacingRight = true;
    private bool _hasJumped;
    private float _currentSpeed;
    
    private bool _canDash = true;
    private bool _isDashing;
    
    private bool _isWallSliding;
    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private float _wallJumpingCounter;
    
    private bool isJumping;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

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
            _currentSpeed = speed;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _currentSpeed = airSpeed;
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping || (Input.GetKeyDown(KeyCode.W) && canDoubleJump && !_hasJumped))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
            _hasJumped = true;
        }
        
        // Make jumps go higher if you press longer
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            print("jumping?");
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _canDash && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        if (canWallJump)
        {
            WallJump();
        }

        if (!_isWallJumping)
        {
            Flip();
        }
    }
    
    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }

    private void FixedUpdate()
    {
        if (_isDashing || _isWallJumping)
        {
            return;
        }
        rb.velocity = new Vector2(_horizontal * _currentSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
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
    
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontal != 0f)
        {
            print(IsWalled());
            _isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
        }
        else
        {
            _isWallSliding = false;
        }
    }
    
    private void WallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -transform.localScale.x;
            _wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W) && _wallJumpingCounter > 0f)
        {
            // reset movement after wall jump
            _canDash = true;
            _hasJumped = false;
            
            _isWallJumping = true;
            rb.velocity = new Vector2(_wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            _wallJumpingCounter = 0f;

            if (transform.localScale.x != _wallJumpingDirection)
            {
                _isFacingRight = !_isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }
    
    private void StopWallJumping()
    {
        _isWallJumping = false;
    }
}