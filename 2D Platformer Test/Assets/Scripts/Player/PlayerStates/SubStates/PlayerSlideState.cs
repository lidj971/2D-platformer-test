using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerGroundedState
{
    //Determine si l'animation de StartSliding est terminer
    private bool isStartAnimationFinished;
    //Determine si l'on est encore sous un block
    private bool isTouchingCeiling;

    
    public PlayerSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    //on appelle cette fonction lorsque StopSliding est terminer
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    //on appelle cette fonction lorsque l'animation StartSlide est finie
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        isStartAnimationFinished = true;
    }

    public override void AnimationUpdate()
    {
        base.AnimationUpdate();
        if (isTouchingCeiling)
        {
            if (!isStartAnimationFinished)
            {
                player.SetAnimationState(player.PLAYER_START_SLIDING, player.BodyAnim);
            }
            else
            {
                player.SetAnimationState(player.PLAYER_SLIDING, player.BodyAnim);
            }
        }
        else
        {
            player.SetAnimationState(player.PLAYER_STOP_SLIDING, player.BodyAnim);
        }
        
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingCeiling = player.CheckIfTouchingCeiling();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetActiveCollider(player.slidingCollider);
        isStartAnimationFinished = false;
        player.Glove.SetActive(false);
        //On enleve l'abiliter de saut
        player.JumpState.SetCanJump(false);
    }

    public override void Exit()
    {
        base.Exit();
        
        player.SetActiveCollider(player.standingCollider);
        player.Glove.SetActive(true);
        //On reactive l'abilite de saut
        player.JumpState.SetCanJump(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isStartAnimationFinished) return;
        player.SetVelocityX(playerData.slideVelocity);

        if (isTouchingCeiling || !isAnimationFinished) return;
        stateMachine.ChangeState(player.IdleState);

    }
}
