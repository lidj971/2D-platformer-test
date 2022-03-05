using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region StateVariables
    public PlayerStateMachine StateMachine { get; private set; }

    #region BaseMovements
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
    #endregion

    #region Skills
    public PlayerWallRunState WallRunState { get; private set; }
    #endregion

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator BodyAnim { get; private set; }
    public Animator GloveAnim { get; private set; }
    public Animator[] allAnimators { get; private set; }
    public InputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public SpriteRenderer SR { get; private set; }
    public GameObject Glove;
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
    public string PLAYER_IDLE { get; private set; } = "Player_Idle";

    public string PLAYER_RUN_START { get; private set; } = "Player_RunStart";
    public string PLAYER_RUN { get; private set; } = "Player_Run";
    public string PLAYER_RUN_STOP { get; private set; } = "Player_RunStop";

    public string PLAYER_START_SLIDING { get; private set; } = "Player_StartSliding";
    public string PLAYER_SLIDING { get; private set; } = "Player_Sliding";
    public string PLAYER_STOP_SLIDING { get; private set; } = "Player_StopSliding";

    public string PLAYER_JUMP { get; private set; } = "Player_Jump";
    public string PLAYER_FALL { get; private set; } = "Player_Fall";
    public string PLAYER_LANDING { get; private set; } = "Player_Landing";

    public string PLAYER_WALL_SLIDE { get; private set; } = "Player_WallSlide";
    public string PLAYER_WALL_GRAB { get; private set; } = "Player_WallGrab";
    public string PLAYER_WALL_CLIMB { get; private set; } = "Player_WallClimb";

    public string PLAYER_LEDGE_CLIMB { get; private set; } = "Player_LedgeClimb";
    public string PLAYER_LEDGE_GRAB { get; private set; } = "Player_LedgeGrab";
    public string PLAYER_LEDGE_HOLD { get; private set; } = "Player_LedgeHold";
    #endregion

    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    public int FacingDirection { get; private set; }
    public string currentAnimationState;
    #endregion

    #region Check Debug Variables
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool isGrounded;
    private bool isTouchingLedge;
    private bool isTouchingLowWall;
    private bool isTouchingCeiling;
    #endregion

    #region Unity Callback Functions 
    private void Awake()
    {
        //Initialisation de la StateMachine
        StateMachine = new PlayerStateMachine();

        //Initialisation des States de base       
        IdleState = GetComponentInChildren<PlayerIdleState>();
        MoveState = GetComponentInChildren<PlayerMoveState>();
        JumpState = GetComponentInChildren<PlayerJumpState>();
        SlideState = GetComponentInChildren<PlayerSlideState>();
        InAirState = GetComponentInChildren<PlayerInAirState>();
        LandState = GetComponentInChildren<PlayerLandState>();
        WallSlideState = GetComponentInChildren<PlayerWallSlideState>();
        WallGrabState = GetComponentInChildren<PlayerWallGrabState>();
        WallClimbState = GetComponentInChildren<PlayerWallClimbState>();
        WallJumpState = GetComponentInChildren <PlayerWallJumpState>();
        LedgeClimbState = GetComponentInChildren<PlayerLedgeClimbState>();

        //Initialisation des Skills
        WallRunState = GetComponentInChildren<PlayerWallRunState>();

        //Intialisation des donnes de tout les States
        PlayerState[] AllStates = GetComponentsInChildren<PlayerState>();
        foreach(PlayerState state in AllStates)
        {
            state.player = this;
            state.stateMachine = StateMachine;
            state.playerData = playerData;
            state.stateName = state.ToString();
            state.stateName = state.stateName.Replace(state.name,"");
            state.stateName = state.stateName.Replace("(Player", "");
            state.stateName = state.stateName.Replace(")", "");
        }
    }

    private void Start()
    {
        //Initialisation des Composant au composant du personage
        BodyAnim = GetComponent<Animator>();
        GloveAnim = Glove.GetComponent<Animator>();
        InputHandler = GetComponentInChildren<InputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        allColliders = GetComponentsInChildren<BoxCollider2D>();
        allAnimators = GetComponentsInChildren<Animator>();


        //Initialison de la Statmachine au state Idle
        StateMachine.Initialize(IdleState);

        SetActiveCollider(standingCollider);
        FacingDirection = 1;
        RB.gravityScale = playerData.gravityScale;
    }

    //Fonction Update appelee 1 fois par frame
    private void Update()
    {
        CurrentVelocity = RB.velocity;
        //Appel des fonctions LogicUpdate et AnimationUpdate du state actuel
        StateMachine.CurrentState.LogicUpdate();
        StateMachine.CurrentState.AnimationUpdate();
        
        //On assigne au varibales de check les fonctions correspondante
        isTouchingWall = CheckIfTouchingWall();
        isTouchingWallBack = CheckIfTouchingWallBack();
        isGrounded = CheckIfGrounded();
        isTouchingLedge = CheckIfTouchingLedge();
        isTouchingLowWall = CheckIfTouchingLowWall();
        isTouchingCeiling = CheckIfTouchingCeiling();

    }
    
    //Fonction FixedUpdate appelee 1 fois par frame a une frame-rate fixe
    private void FixedUpdate()
    {
        //Appel de la fonction Physics Update du state actuel
        StateMachine.CurrentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions 

    //permet de modifier la velocite a un angle precis
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    //permet d'augmenter progressivement la velocite horizontal jusqu'a une limite precise
    public void SetVelocityX(float velocity, float horizontalDamping)
    {
        workspace.Set(RB.velocity.x + velocity * InputHandler.NormInputX, CurrentVelocity.y);
        workspace.x += InputHandler.NormInputX;
        workspace.x *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    //permet de modifier la velocite horizontal d'un coup
    public void SetVelocityX(float velocity)
    {
        workspace.Set(FacingDirection * velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetWallRunVelocity(float velocity, float verticalDamping)
    {
        workspace.Set(RB.velocity.x, RB.velocity.y + velocity);
        workspace.y += 1;
        workspace.y *= Mathf.Pow(1f - verticalDamping, Time.deltaTime * 10f);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    //permet de modifier la velocite vertical d'un coup
    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    //permet de changer l'animation joue par tout les animateurs du personage
    public void SetAnimationState(string newState)
    {
        foreach(Animator anim in allAnimators)
        {
            anim.Play(newState);
        }
        currentAnimationState = newState;
    }

    //permet de changer l'animation joue par un animateur precis
    public void SetAnimationState(string newState,Animator animator)
    {
        animator.Play(newState);
        currentAnimationState = newState;
    }

    //permet de changer l'animation joue par tour les Animateurs du personage a une frame precise
    public void SetAnimationState(string newState,int totalFrames,int frame)
    {
        foreach(Animator anim in allAnimators)
        {
            anim.Play(newState, 0, (1f / totalFrames) * frame);
        }
        currentAnimationState = newState;
    }

    //permet de changer l'animation joue par un animateur precis a une frame precise
    public void SetAnimationState(string newState, int totalFrames, int frame,Animator animator)
    {
        animator.Play(newState, 0, (1f / totalFrames) * frame);
        currentAnimationState = newState;
    }

    //permet de changer la collision active
    public void SetActiveCollider(BoxCollider2D activeCollider)
    {
        foreach(BoxCollider2D collider in allColliders)
        {
            collider.enabled = false;
        }

        activeCollider.enabled = true;
    }

    public void SetGravityScale(float gravity)
    {
        RB.gravityScale = gravity;
    }
    #endregion

    #region Check Functions
    //Verifie si l'on est au sol
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    //Verifie si l'on Touche un mur
    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    //Verifie si l'on Touche un Bord
    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    //Verifie si l'on est dos a un mur
    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    //Verifie si le bas du personage touche un mur
    public bool CheckIfTouchingLowWall()
    {
        return Physics2D.Raycast(lowWallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    }

    //Verifie si le personage touche le plafond pendant qu'il slide
    public bool CheckIfTouchingCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    //Verifie si le personage devrait se retourner
    public void CheckIfShouldFlip(int xInput)
    {
        //Si la direction de l'input est differente de la direction du personnage on retourne le personage
        if (xInput == 0 || xInput == FacingDirection) return;
        Flip();
    }
    #endregion

    #region Other Functions
    private void AnimationTriggerFunction() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    
    //retourne le personage de 180 degres sur l'axe y
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    
    //Met la velocite horizontale a 0
    public void KillVelocityX()
    {
        workspace.Set(0f, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    
    //Met la velocite a 0
    public void KillVelocity()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    //Determine la position du coin du bord lorsqu'on ledgeHold
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
