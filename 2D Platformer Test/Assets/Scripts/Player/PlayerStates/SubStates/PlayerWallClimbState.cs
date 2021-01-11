using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetAnimationState(player.PLAYER_WALLCLIMB);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.SetVelocityY(playerData.wallClimbVelocity);

        if(yInput != 1)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }
}
