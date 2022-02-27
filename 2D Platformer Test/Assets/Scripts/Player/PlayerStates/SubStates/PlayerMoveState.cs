﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
   
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.CheckIfShouldFlip(xInput);

        if (!isExitingState)
        {
            if (player.CurrentVelocity.x <= 0.1 && player.CurrentVelocity.x >= -0.1 && !isExitingState && xInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();

        if (xInput != 0)
        {
            player.SetAnimationState(player.PLAYER_RUN);
        }
        else
        {
            player.SetAnimationState(player.PLAYER_IDLE);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        player.SetVelocityX(playerData.movementVelocity,playerData.horizontalDamping);
    }
}