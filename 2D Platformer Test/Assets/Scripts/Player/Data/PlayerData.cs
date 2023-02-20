using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    
    [Header("Physics")]
    public float gravityScale = 3f;
    public float fallGravityMultiplier = 1.3f;
    
    [Header("Move")]
    public float movementVelocity = 10f;
    [Range(0, 1)]
    public float horizontalDamping = 0.5f;

    [Header("Jump")]
    public float jumpVelocity = 15f;
    public float minJumpVelocity = 5f;
    public int amountOfJumps = 1;

    [Header("Slide")]
    public float minVelocity = 3f;
    public float slideVelocity = 10f;
    public float slideTime = 0.5f;


    [Header("Dash")]
    public float dashVelocity = 10f;
    public float dashTime = 0.5f;
    public float dashCoolDown = 0.5f;
    public float drag = 10f;
    public float dashEndYMultiplier = 0.2f;
    public float distBetweenAfterImages = 0.5f;

    [Header("Wall Jump")]
    public float wallJumpVelocity = 20;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("In Air")]
    public float coyoteTime = 0.2f;
    [Range(0, 1)]
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide")]
    public float wallSlideVelocity = 3f;

    [Header("Wall Climb")]
    public float wallClimbVelocity = 3f;

    [Header("Ledge Climb")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("Check Variables")]
    public float groundCheckExtraHeight = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;


}
