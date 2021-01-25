using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    
    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    public bool isTouchingElevador;
    private bool isWallSliding;
    //private bool canJump;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool isTouchingLedge;
    private bool canClimbLedge = false;
    private bool ledgeDetected;
    private bool isDashing;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private Rigidbody2D rb;
    private Animator anim;

    public int amountOfJumps = 1;

    public float movementSpeed = 10;
    public float jumpForce = 16;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;

    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;
    public float dashTime = 0.2f;
    public float dashSpeed = 50f;
    public float distanceBetweenImages = 0.1f;
    public float dashCoolDown = 2.5f;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsElevador;

    private bool spawnDust;
    public GameObject walkeffect;
    public GameObject jumpeffect;

    //public Animator camAnim;

    // public AudioSource andado;
    // public AudioSource pulo;
    // public AudioSource dashSound;
    // public AudioSource slidingSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();

        // if(isWalking && isGrounded && !canClimbLedge)
        // {
        //     andado.volume = 1;
        // }else{
        //     andado.volume = 0;
        // }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurrondings();
    }

    private void CheckIfWallSliding()
    {
        if(isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0 && !canClimbLedge) 
        {
            isWallSliding = true;
        }else{
            isWallSliding = false;
        }
    }

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if(isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            anim.SetBool("canClimbLedge", canClimbLedge);
        }

        if(canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);  
    }

    private void CheckSurrondings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        // if(isGrounded)
        // {
        //     if(spawnDust)
        //     {
        //         Instantiate(walkeffect, groundCheck.position, Quaternion.identity);
        //         spawnDust = false;
        //     }
        // }else{
        //     spawnDust = true;
        // }

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        isTouchingElevador = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsElevador);

        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsGround);

        if(isTouchingWall && !isTouchingLedge && !ledgeDetected && !isTouchingElevador)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if(isTouchingWall){
            canWallJump = true;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }else{
            canNormalJump = true;
        }
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(movementInputDirection != 0)
        {
            isWalking = true;
            
            
        }else{
            
            isWalking = false;
        }

        if(isWalking && isGrounded && !isWallSliding)
        {
            Instantiate(walkeffect, groundCheck.position, Quaternion.identity);
        }
        
        if(isGrounded && !canClimbLedge && !isWallSliding)
        {
            if(spawnDust)
            {
                //camAnim.SetTrigger("shake");
                Instantiate(jumpeffect, groundCheck.position, Quaternion.identity);
                spawnDust = false;
                
            }
        }else{
            spawnDust = true;
        }

        // if(isWallSliding)
        // {
        //     slidingSound.Play();
        // }else{
        //     slidingSound.Pause();
        // }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            //Jump();
            

            if(isGrounded || amountOfJumpsLeft > 0 && isTouchingWall)
            {
                NormalJump();
            }else{
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if(turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if(checkJumpMultiplier && !Input.GetButton("Jump")){
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if(Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (lastDash + dashCoolDown)){
                AttemptToDash();
            }
        }
    }

    private void AttemptToDash()
    {
        //dashSound.Play();
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        //PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void CheckDash()
    {
        if(isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if(Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    //PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
            
        }
    }

    private void CheckJump()
    {

        if(jumpTimer > 0)
        {
            if(!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection){
                WallJump();
            }else if(isGrounded){
                NormalJump();
                
            }
        }

        if(isAttemptingToJump){
            jumpTimer -= Time.deltaTime;
        }
        
        // else if(isWallSliding && movementInputDirection == 0 && canJump)//wall hop
        // {   	
        //     isWallSliding = false;
        //     amountOfJumpsLeft--;
        //     Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
        //     rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        // }
        
    }

    private void NormalJump()
    {
        if(canNormalJump)
        { 
            //pulo.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if(canWallJump)
        {
            //pulo.Play();
            rb.velocity = new Vector2(rb.velocity.x, 0.0f); 
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }
    }

    private void ApplyMovement()
    {

        if(!isGrounded && !isWallSliding && movementInputDirection == 0){
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if(canMove){
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        // else if(!isGrounded && !isWallSliding && movementInputDirection != 0){
        //     Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
        //     rb.AddForce(forceToAdd);

        //     if(Mathf.Abs(rb.velocity.x) > movementSpeed){
        //         rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        //     }
        // }
        

        if(isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Flip()
    {
        if(!isWallSliding && canFlip){
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f,180.0f,0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
