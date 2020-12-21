using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingWall;
    private bool isOnWall;
    private bool isOnSurface;
    private bool isOnLeftWall;
    private bool isTouchingGround;
    private bool backIsTouchingGround;
    private bool isRunning;
    private bool hasJumped;

    public float amountOfJumpsleft;
    public float GroundedRemember;
    
    public float horizontalVelocity;

    public PlayerData playerData;
    private InputHandler inputHandler;
    private Rigidbody2D rb;
    public Transform groundCheck, wallCheck;



    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody2D>();
        amountOfJumpsleft = playerData.amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfCanJump();

    }

    void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void ApplyMovement()
    {
        if(inputHandler.RunInput && isGrounded)
        {
          horizontalVelocity = rb.velocity.x + inputHandler.NormInputX * playerData.runningVelocity;
        }
        else
        {
          horizontalVelocity = rb.velocity.x + inputHandler.NormInputX * playerData.movementVelocity;
        }
        
        horizontalVelocity += inputHandler.NormInputX;
        horizontalVelocity *= Mathf.Pow(1f - playerData.horizontalDamping, Time.deltaTime * 10f);
        
        
        
        rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
    }

    private void CheckIfCanJump()
    {
        GroundedRemember -= Time.deltaTime;
        if (isGrounded)
        {
            GroundedRemember = playerData.coyoteTime;
            amountOfJumpsleft = playerData.amountOfJumps;
        }
        
        if (GroundedRemember > 0 || amountOfJumpsleft > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        
        if (inputHandler.JumpInput && canJump)
        {
            GroundedRemember = 0;                               
            Jump();
                     
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);

        if (isGrounded || isOnWall)
        {
            isOnSurface = true;
        }
        else
        {
            isOnSurface = false;
        }
    }

    private void Jump()
    {
        
        rb.velocity = new Vector2(rb.velocity.x, playerData.jumpVelocity);
        amountOfJumpsleft--;
        



        if (inputHandler.JumpInputStop && rb.velocity.y > 0)
        {
            Debug.Log("lol");
            rb.velocity = new Vector2(rb.velocity.x, playerData.jumpVelocity * playerData.variableJumpHeightMultiplier);
           
        }

        
        
    }

}
