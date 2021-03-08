using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    private string stateName;

    protected float startTime;

    protected bool isAnimationFinished;
    protected bool isExitingState;
    


    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.stateName = stateName;
    }


    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        Debug.Log(stateName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void LogicUpdate() { }

    public virtual void AnimationUpdate() { }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
