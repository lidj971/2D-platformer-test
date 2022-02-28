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

    //Fonction appelee lorsqu'on rentre dans un state
    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        Debug.Log(stateName);
        isAnimationFinished = false;
        isExitingState = false;
    }

    //Fonction appelee losque l'on quitte un state
    public virtual void Exit()
    {
        isExitingState = true;
    }

    //Fonction dans laquelle on fait ce qui touche a la logique
    public virtual void LogicUpdate() { }

    //Fonction dans laquelle on fait ce qui touche aux animations
    public virtual void AnimationUpdate() { }

    //Fonction dans laquelle on fait ce qui touche a la physique
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    //Fonction dans laquelle on fait nos Verifications
    public virtual void DoChecks() { }

    //Fonction Appelee a une frame precise d'une animation
    public virtual void AnimationTrigger() { }

    //Fonction determinant qu'un animation est finie
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
