﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            player.SetVelocityY(-playerData.wallSlideVelocity);

            if (grabInput && yInput == 0)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
        }
       
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        player.SetAnimationState(player.PLAYER_WALLSLIDE);
    }
}