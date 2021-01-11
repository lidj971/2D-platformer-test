using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName) : base(player, stateMachine, playerData, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetAnimationState(player.PLAYER_WALLGRAB);
        player.FreezePosition();
    }

    public override void Exit()
    {
        base.Exit();
        player.ResetConstraints();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.KillVelocityX();
        player.SetVelocityY(0f);

        if(yInput > 0)
        {
            stateMachine.ChangeState(player.WallClimbState);
        }else if(yInput < 0 || !grabInput)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    

}
