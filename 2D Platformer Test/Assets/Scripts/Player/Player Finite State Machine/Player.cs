using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Mirror;

public class Player : NetworkBehaviour
{
    #region StateVariables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }

    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallGrabState WallGrabState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }

    public PlayerLedgeClimbState LedgeClimbState { get; private set; }


    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;
    #endregion

    #region Animation Names
    public string PLAYER_IDLE { get; private set; }
    public string PLAYER_RUNSTART { get; private set; }
    public string PLAYER_RUN { get; private set; }
    public string PLAYER_RUNSTOP { get; private set; }
    public string PLAYER_JUMP { get; private set; }
    public string PLAYER_JUMPTOFALL { get; private set; }
    public string PLAYER_LANDING { get; private set; }
    public string PLAYER_WALLSLIDE { get; private set; }
    public string PLAYER_WALLGRAB { get; private set; }
    public string PLAYER_WALLCLIMB { get; private set; }
    public string PLAYER_LEDGECLIMB { get; private set; }
    public string PLAYER_LEDGEGRAB { get; private set; }
    public string PLAYER_LEDGEHOLD { get; private set; }

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    [HideInInspector]
    public Vector2 workspace;
    public SpriteRenderer ui;
    public GameObject dot;
    #endregion

    #region Unity Callback Functions 
    private void Awake()
    {
        //if (this.isLocalPlayer)
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "jump");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "WallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "WallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "WallJump");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "LedgeClimb");

            PLAYER_IDLE = "Player_Idle";
            PLAYER_RUNSTART = "Player_RunStart";
            PLAYER_RUN = "Player_Run";
            PLAYER_RUNSTOP = "Player_RunStop";
            PLAYER_JUMP = "Player_Jump";
            PLAYER_JUMPTOFALL = "Player_JumpToFall";
            PLAYER_LANDING = "Player_Landing";
            PLAYER_WALLSLIDE = "Player_WallSlide";
            PLAYER_WALLGRAB = "Player_WallGrab";
            PLAYER_WALLCLIMB = "Player_WallClimb";
            PLAYER_LEDGEGRAB = "Player_LedgeGrab";
            PLAYER_LEDGEHOLD = "Player_LedgeHold";
            PLAYER_LEDGECLIMB = "Player_LedgeClimb";

        }
        
    }


    private void Start()
    {
        //if (this.isLocalPlayer)
        {
            FacingDirection = 1;

            Anim = GetComponent<Animator>();

            InputHandler = GetComponent<InputHandler>();

            RB = GetComponent<Rigidbody2D>();

            StateMachine.Initialize(IdleState);

            SR = GetComponent<SpriteRenderer>();
        }
        
    }

    private void Update()
    {
        //if (this.isLocalPlayer)
        {
            CurrentVelocity = RB.velocity;
            StateMachine.CurrentState.LogicUpdate();
            if (this.isLocalPlayer)
            {
                ui.color = new Color(0, 0, 255);

            }
            else
            {
                ui.color = new Color(255, 0, 0);

            }
        }
    }


    private void FixedUpdate()
    {
        //if (this.isLocalPlayer)
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
    }
    #endregion

    #region Set Functions 
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        if (this.isLocalPlayer)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }


    public void SetVelocityX(float velocity, float horizontalDamping)
    {
        if (this.isLocalPlayer)
        {
            workspace.Set(RB.velocity.x + velocity * InputHandler.NormInputX, CurrentVelocity.y);
            workspace.x += InputHandler.NormInputX;
            workspace.x *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    public void SetVelocityY(float velocity)
    {
        if (this.isLocalPlayer)
        {
            workspace.Set(CurrentVelocity.x, velocity);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
        
    }

    public void SetAnimationState(string newState) 
    {
        if (this.isLocalPlayer)
        {
            Anim.Play(newState);
        }

    }
    #endregion

    #region Check Functions
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
    #endregion

    #region Other Functions
    private void AnimationTriggerFunction() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    
    public void KillVelocityX()
    {
        workspace.Set(0f, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    
    public void KillVelocity()
    {
        
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
        float xDist = xHit.distance;
        workspace.Set(xDist * FacingDirection,0f);
        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace), Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDist = yHit.distance;
        workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
        return workspace;
    }
    

    #endregion
}
