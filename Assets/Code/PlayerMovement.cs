using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    private CapsuleCollider2D playerCollider;
    private BoxCollider2D playerCollider2;
    public CapsuleCollider2D rollCollider;
    private TrailRenderer tr;
    private GameObject currentOneWayPlatform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);

    [SerializeField] public Vector2 moveInput;
    [SerializeField] bool currentJumpUp = false;
    [SerializeField] UnityEvent jumpEvent;
    [SerializeField] UnityEvent landEvent;
    [SerializeField] UnityEvent runEvent;
    private Animator anim;

    #region  Variables float
    [Header("跑步")]
    public float runMaxSpeed;
    public float runAcceleration;
    private float runAccelAmount;
    public float runDecceleration;
    private float runDeccelAmount;
    [Space(5)]

    [Header("跳躍")]
    public float jumpHeight;
    public float jumpTimeToApex;
    private float jumpForce;
    public float jumpCutMulti;
    public float doubleJumpMulti;
    [Space(5)]

    [Header("翻滾")]
    public float rollPower;
    public float rollTime;
    public float rollCoolDownTime;
    [Space(5)]

    [Header("重力")]
    public float fallGravityMult;
    private float gravityScale;
    private float gravityStrength;
    public float maxFallSpeed;
    [Space(5)]
    

    [Header("時間")]
    public float coyoteTime;
    private float coyoteTimeCounter;
    public float JumpBufferTime;
    private float jumpBufferTimeCounter;
    private float knockbackStartTime;
    public float knockbackDuration;
    public Vector2 knockbackSpeed;
    public float hurtTime;
    #endregion
    [Space(5)]

    #region Variables bool
    [Header("能力")]
    public bool canDoubleJump;
    public bool canRoll;
    //[Space(5)]

    //[Header("檢查")]
    public bool isGround { get; private set; }
    public bool isFacingRight { get; private set; }
    public bool isJumping { get; private set; }
    public bool isJumpPressed { get; private set; }
    public bool isJumpCut { get; private set; }
    public bool isFalling { get; private set; }
    public bool isDoubleJump { get; private set; }
    public bool isRolling { get; private set; }
    public bool isDisable { get; private set; }
    public bool isKnockBack { get; private set; }
    public bool isHurt { get; private set; }

    private bool canFlip;

    #endregion


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        playerCollider2 = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        canFlip = true;
        isFacingRight = true;
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
        gravityScale = gravityStrength / Physics2D.gravity.y;
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;
    }

    public void OnMove(InputValue inputValue)
    {
        var tempInput = inputValue.Get<Vector2>();
        currentJumpUp = moveInput.y >= 0.5f && tempInput.y == 0;
        moveInput = tempInput;
    }

    private void Update()
    {
        //Timer
        coyoteTimeCounter -= Time.deltaTime;
        jumpBufferTimeCounter -= Time.deltaTime;

        #region Input

        //Input Jump
        if (moveInput.y >= 0.5f)
        {
            jumpBufferTimeCounter = JumpBufferTime;
            isJumpPressed = true;
        }
        //Input JumpCut
        if (currentJumpUp)
        {
            jumpCut();
            isJumpPressed = false;
            currentJumpUp = false;
        }
        //Input down the platfrom
        //if (Input.GetKey(KeyCode.S) && Input.GetButtonDown("Jump"))
        //{
        //    JumpDown();
        //}
        //Input Roll
        //if (Input.GetKeyDown(KeyCode.LeftShift) && !Input.GetButtonDown("Jump"))
        //{
        //    Roll();
        //}
        #endregion

        #region Check
        //Check if is on the ground
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;

            if (isFalling)
            {
                anim.SetBool("isJump", false);
                landEvent?.Invoke();
            }
            isDoubleJump = canDoubleJump ? true : false;
            isFalling = false;
            isJumpCut = false;
            isGround = true;

            //Check if the floor is OneWayPlatform
            IsOneWayPlatform();
        }
        else
        {
            isGround = false;
        }
        //Check if is Falling
        if (isJumping && !isRolling && RB.velocity.y < 0)
        {
            isFalling = true;
            isJumping = false;
        }
        //Check if need to change facing direction
        if (canFlip && !isKnockBack)
        {
            if (isFacingRight && moveInput.x < 0 || !isFacingRight && moveInput.x > 0)
                Flip();
        }
        //Check to prevent bufferjump not getting jumpCut
        if (isJumping && moveInput.y <= 0.5f)
        {
            isJumpCut = true;
        }

        CheckKnockBack();
        CheckHurt();

        #endregion

        #region Jump
        
        //Jump and Double Jump
        if (coyoteTimeCounter > 0 && jumpBufferTimeCounter > 0 && !isJumping && !isJumpCut && !isRolling)
        {
            Jump(jumpForce);
        }
        else if (canDoubleJump && !isRolling)
        {
            DoubleJump();
        }

        #endregion

        #region Gravity
        //Higher gravity if falling
        if (RB.velocity.y < 0)
        {
            SetGravityScale(gravityScale * fallGravityMult);
            //Max falling speed
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
        }
        else if (isJumpCut)
        {
            SetGravityScale(gravityScale * jumpCutMulti);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
        }
        else if (!isJumping && isGround && !isHurt)
        {
            SetGravityScale(0);
        }
        else
        {
            //Set gravity to normal
            SetGravityScale(gravityScale);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (!isRolling && !isKnockBack)
            Run();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Run()
    {
        //The final speed we want to reach
        float targetSpeed;
        targetSpeed = moveInput.x * runMaxSpeed;
        //Caculate difference between current and target speed
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        //50 is the number of times a second Unity calls it physics update => 1 / 0.02
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        //Caculate how much force we still need for the velocity
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right);

        Debug.Log(IsGrounded() + "/" + moveInput.x);
        anim.SetBool("isRun", IsGrounded() && moveInput.x != 0);
    }

    private void Jump(float force)
    {
        jumpBufferTimeCounter = 0;

        isJumping = true;
        isJumpCut = false;
        isFalling = false;

        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        jumpEvent?.Invoke();
        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        anim.SetBool("isJump", true);
    }

    private void DoubleJump()
    {
        if (isDoubleJump && jumpBufferTimeCounter > 0 && !isJumping)
        {
            isDoubleJump = !isDoubleJump;
            Jump(jumpForce * doubleJumpMulti);
        }
    }

    private void jumpCut()
    {
        if (isJumping && RB.velocity.y > 0)
        {
            isJumpCut = true;
        }
    }

    private void JumpDown()
    {
        if (currentOneWayPlatform != null)
        {
            //Disable both player and platform collidison
            StartCoroutine(DisableCollision());
        }
    }

    public void Knockback(int direction)
    {
        isKnockBack = true;
        isHurt = true;
        knockbackStartTime = Time.time;
        RB.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    private void CheckKnockBack()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && isKnockBack)
        {
            isKnockBack = false;
            RB.velocity = new Vector2(0.0f, RB.velocity.y);
        }
    }

    private void CheckHurt()
    {
        if (Time.time >= knockbackStartTime + hurtTime && isHurt)
        {
            isHurt = false;
        }
    }

    private void Roll()
    {
        if (canRoll && !isJumping && !isFalling && moveInput.x != 0 && !isKnockBack)
        {
            StartCoroutine(PerformRoll());
        }
    }

    private IEnumerator PerformRoll()
    {
        canRoll = false;
        isRolling = true;

        SetGravityScale(0f);
        playerCollider.enabled = false;
        playerCollider2.enabled = false;
        RB.velocity = new Vector2(moveInput.x * rollPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(rollTime);
        tr.emitting = false;
        SetGravityScale(gravityScale);
        playerCollider.enabled = true;
        playerCollider2.enabled = true;
        isRolling = false;
        yield return new WaitForSeconds(rollCoolDownTime);
        canRoll = true;
        isJumping = false;
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        isDisable = true;
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        Physics2D.IgnoreCollision(playerCollider2, platformCollider);
        Physics2D.IgnoreCollision(rollCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        isDisable = false;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        Physics2D.IgnoreCollision(playerCollider2, platformCollider, false);
        Physics2D.IgnoreCollision(rollCollider, platformCollider, false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
    }

    private void IsOneWayPlatform()
    {
        Collider2D collider = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
        if (collider.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collider.gameObject;
        }
        else
        {
            currentOneWayPlatform = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    #region Public
    public void EnableFlip()
    {
        canFlip = true;
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public bool GetFacingDirection()
    {
        return isFacingRight;
    }

    public int GetFacingInt()
    {
        if (isFacingRight)
            return 1;
        else
            return -1;
    }
    #endregion
}
