using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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



    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region Check Transforms
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform wallCheck;
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

    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }
    [HideInInspector]
    public Vector2 workspace;
    #endregion

    #region Unity Callback Functions //in all set functions add  if(this.isLocalPlayer) 
    private void Awake()
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
    }


    private void Start()
    {
        FacingDirection = 1;

        Anim = GetComponent<Animator>();

        InputHandler = GetComponent<InputHandler>();

        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }


    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions 
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace = new Vector2(angle.x * velocity * direction, angle.y * velocity);
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

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetAnimationState(string newState) 
    {
        Anim.Play(newState);
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
    public void FreezePosition()
    {
        RB.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void ResetConstraints()
    {
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    #endregion
}
