using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canDoubleJump;
    public bool canDash;
    public bool canWallJump;
    public bool canSuperDash;
    public bool canDownDash;
    
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
    [SerializeField] private float superDashPower = 40f;
    [SerializeField] private float superDashStartup = 1f;
    [SerializeField] private float downDashPower = 50f;
    [SerializeField] private float downDashStartup = 0.25f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform wallCheck2;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private TrailRenderer superTrail;

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
    
    private bool _isJumping;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    private bool _isSuperDashing;
    private float _originalGravity;
    private bool _hasLeftWall;

    private bool _isDownDashing;

    private void Start()
    {
        _originalGravity = rb.gravityScale;
    }

    void Update()
    {
        if ((IsDoubleWalled() && _isSuperDashing && _hasLeftWall) || (IsGrounded() && _isDownDashing))
        {
            ResetSuperDash();
        }

        if (!IsWalled() && _isSuperDashing)
        {
            _hasLeftWall = true;
        }

        if (_isDashing || _isSuperDashing || _isDownDashing)
        {
            return;
        }
        
        _horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            _hasJumped = false;
            _canDash = true;
            _currentSpeed = speed;
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _currentSpeed = airSpeed;
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        
        if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && !_isJumping || (Input.GetKeyDown(KeyCode.W) && canDoubleJump && !_hasJumped))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            _jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
            _hasJumped = true;
        }
        
        // Make jumps go higher if you press longer
        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            _coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.S) && canDownDash)
            {
                StartCoroutine(DownDash());
            }
            else if (_isWallSliding && canSuperDash)
            {
                StartCoroutine(SuperDash());
            }
            else if (_canDash && canDash)
            {
                StartCoroutine(Dash());
            }
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
        _isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        _isJumping = false;
    }

    private void FixedUpdate()
    {
        if (_isDashing || _isWallJumping || _isSuperDashing)
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

    private bool IsDoubleWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer) || Physics2D.OverlapCircle(wallCheck2.position, 0.2f, wallLayer);
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
        if (_isSuperDashing) yield return null;

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

    private IEnumerator SuperDash()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(superDashStartup);
        // if (!IsGrounded() && IsWalled() && !_hasChargingLeftWall)
        _canDash = false;
        _isSuperDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * superDashPower * -1, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        superTrail.emitting = true;
    }

    private IEnumerator DownDash()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(downDashStartup);
        _canDash = false;
        _isDownDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f, transform.localScale.y * downDashPower * -1);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        superTrail.emitting = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        ResetSuperDash();
    }

    private void ResetSuperDash()
    {
        _isDownDashing = false;
        
        _isSuperDashing = false;
        rb.gravityScale = _originalGravity;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _hasLeftWall = false;
        superTrail.emitting = false;
        
        // not sure if this will be ok
        _canDash = true;
        _hasJumped = false;
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && _horizontal != 0f)
        {
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