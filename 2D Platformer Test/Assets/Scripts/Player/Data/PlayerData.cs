﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move")]
    public float movementVelocity = 10f;
    [Range(0, 1)]
    public float horizontalDamping = 0.5f;

    [Header("Jump")]
    public float jumpVelocity = 15f;
    public float minJumpVelocity = 5f;
    public int amountOfJumps = 1;

    [Header("Slide")]
    public float slideVelocity = 10f;

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
    public float groundCheckRadius = 0.3f;
    public float wallCheckDistance = 0.5f;
    public LayerMask whatIsGround;


}
