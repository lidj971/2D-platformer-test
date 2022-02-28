using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region StateVariables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerSlideState SlideState { get; private set; }

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

    #region Colliders
    public BoxCollider2D standingCollider;
    public BoxCollider2D slidingCollider;

    public BoxCollider2D[] allColliders { get; private set; }

    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;

    [SerializeField]
    private Transform ceilingCheck;

    [SerializeField]
    private Transform lowWallCheck;
    #endregion

    #region Animation Names
    public string PLAYER_IDLE { get; private set; }
    public string PLAYER_RUN_START { get; private set; }
    public string PLAYER_RUN { get; private set; }
    public string PLAYER_RUN_STOP { get; private set; }
    public string PLAYER_START_SLIDING { get; private set; }
    public string PLAYER_SLIDING { get; private set; }
    public string PLAYER_STOP_SLIDING { get; private set; }
    public string PLAYER_JUMP { get; private set; }
    public string PLAYER_JUMPTOFALL { get; private set; }
    public string PLAYER_FALL { get; private set; }
    public string PLAYER_LANDING { get; private set; }
    public string PLAYER_WALL_SLIDE { get; private set; }
    public string PLAYER_WALL_GRAB { get; private set; }
    public string PLAYER_WALL_CLIMB { get; private set; }
    public string PLAYER_LEDGE_CLIMB { get; private set; }
    public string PLAYER_LEDGE_GRAB { get; private set; }
    public string PLAYER_LEDGE_HOLD { get; private set; }
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    public int FacingDirection { get; private set; }
    public string currentAnimationState;


    #endregion

    public bool isTouchingLowWall;
    public bool isTouchingWall;
    public bool isTouchingCeiling;

    #region Unity Callback Functions 
    private void Awake()
    {
        
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "jump");
        SlideState = new PlayerSlideState(this, StateMachine, playerData, "slide");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "WallSlide");
        WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "WallGrab");
        WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "WallClimb");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "WallJump");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "LedgeClimb");

        PLAYER_IDLE = "Player_Idle";
        PLAYER_RUN_START = "Player_RunStart";
        PLAYER_RUN = "Player_Run";
        PLAYER_RUN_STOP = "Player_RunStop";
        PLAYER_START_SLIDING = "Player_StartSliding";
        PLAYER_SLIDING = "Player_Sliding";
        PLAYER_STOP_SLIDING = "Player_StopSliding";
        PLAYER_JUMP = "Player_Jump";
        PLAYER_FALL = "Player_Fall";
        PLAYER_LANDING = "Player_Landing";
        PLAYER_WALL_SLIDE = "Player_WallSlide";
        PLAYER_WALL_GRAB = "Player_WallGrab";
        PLAYER_WALL_CLIMB = "Player_WallClimb";
        PLAYER_LEDGE_GRAB = "Player_LedgeGrab";
        PLAYER_LEDGE_HOLD = "Player_LedgeHold";
        PLAYER_LEDGE_CLIMB = "Player_LedgeClimb";
    }


    private void Start()
    {

        FacingDirection = 1;

        Anim = GetComponent<Animator>();

        InputHandler = GetComponent<InputHandler>();

        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);

        SR = GetComponent<SpriteRenderer>();

        allColliders = GetComponentsInChildren<BoxCollider2D>();

    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
        StateMachine.CurrentState.AnimationUpdate();
        isTouchingLowWall = CheckIfTouchingLowWall();
        isTouchingWall = CheckIfTouchingWall();
        isTouchingCeiling = CheckIfTouchingCeiling();
    }


    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions 

    //set the velocity so that you walljump at an angle

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity, float horizontalDamping)
    {
        workspace.Set(RB.velocity.x + velocity * InputHandler.NormInputX, CurrentVelocity.y);
        workspace.x += InputHandler.NormInputX;
        workspace.x *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(FacingDirection * velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetAnimationState(string newState)
    {
        Anim.Play(newState);
        currentAnimationState = newState;
    }

    public void SetAnimationState(string newState,int totalFrames,int frame)
    {
        Anim.Play(newState, 0, (1f / totalFrames) * frame);
        currentAnimationState = newState;
    }

    public void SetActiveCollider(BoxCollider2D activeCollider)
    {
        foreach(BoxCollider2D collider in allColliders)
        {
            collider.enabled = false;
        }

        activeCollider.enabled = true;
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

    public bool CheckIfTouchingLowWall()
    {
        return Physics2D.Raycast(lowWallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
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

    //determines the position of the corner of the ledge while ledgeholding
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
