using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallRunState : PlayerTouchingWallState
{
    
    /*public PlayerWallRunState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }*/

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetGravityScale(0f);
        
    }

    public override void Exit()
    {
        base.Exit();
        player.SetGravityScale(playerData.gravityScale);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }else if (isTouchingWall && GrabInput)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        player.SetWallRunVelocity(playerData.movementVelocity, playerData.horizontalDamping);
        base.PhysicsUpdate();
    }

    

}
